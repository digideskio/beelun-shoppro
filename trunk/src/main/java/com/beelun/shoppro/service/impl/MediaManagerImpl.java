package com.beelun.shoppro.service.impl;

import java.io.File;
import java.io.FileOutputStream;
import java.io.OutputStream;
import java.util.Arrays;
import java.util.List;

import org.apache.commons.lang.RandomStringUtils;
import org.hibernate.Criteria;
import org.hibernate.Query;
import org.hibernate.SessionFactory;
import org.hibernate.criterion.Criterion;
import org.hibernate.criterion.LogicalExpression;
import org.hibernate.criterion.Restrictions;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.beelun.shoppro.dao.hibernate.GenericDaoHibernate;
import com.beelun.shoppro.model.Media;
import com.beelun.shoppro.service.MediaManager;

@Service(value = "mediaManager")
public class MediaManagerImpl extends GenericDaoHibernate<Media, Long>
		implements MediaManager {

	@Autowired
	public MediaManagerImpl(SessionFactory sessionFactory) {
		super(Media.class, sessionFactory);
	}

	@Override
	public List<Media> getByCondition(String orderBy, boolean ascending,
			int firstResult, int maxResult) {
		return super.getByCondition(orderBy, ascending, firstResult, maxResult);
	}

	@Override
	public int getAllCount() {
		return super.getAllCount();
	}

	@SuppressWarnings("unchecked")
	@Override
	public boolean removeMany(Long[] mediaIdList) {
		log.debug("in media manager remove by idList()...");
		if (mediaIdList != null && mediaIdList.length != 0) {
			try {
				// Delete files physically
				for (Long mediaId : mediaIdList) {
					File f = new File(get(mediaId).getFileAbsolutePath());
					if (f.exists()) {
						boolean deleteOK = f.delete();
						// TODO FIXME: File is not deleted. Probably because of
						// the front end is viewing them
						// Possible fix:
						// (1) Add this to a db table delete_me, and delete them
						// during servlet destroy
						// (2) Find a way to release the images from its opening
						// process
						if (deleteOK == false) {
							log.warn("File is not deleted: " + f.getName());
						}
					}
				}

				// Remove FK to these guys and then delete them
				List<Media> l = (List<Media>) getHibernateTemplate()
						.findByNamedQueryAndNamedParam(
								"MediaManager.getByIdList", "idList",
								Arrays.asList(mediaIdList));
				if (l != null && l.size() != 0) {
					getHibernateTemplate().deleteAll(l);
					getHibernateTemplate().flush(); // Commit the change now
				}
			} catch (Exception ex) {
				log.error(ex.getMessage());
				return false;
			}
		}
		return true;
	}

	@Override
	public Media save(Media media) {
		Media m = null;
		try {
			media.setUpdated();
			m = super.save(media);
		} catch (Exception ex) { // Catch all exception. Ugly, but works. Polish
									// it later on.
			log.error(ex.getMessage());
		}

		return m;
	}

	/**
	 * Verify: (1) fileName is not null and unique (2) title is not null
	 * 
	 * @param media
	 * @return true is passed, otherwise false
	 */
	@SuppressWarnings("unchecked")
	private boolean verifyConstaint(Media media) {
		if (media.getFileName() == null || media.getTitle() == null) {
			return false;
		}

		List l = getHibernateTemplate().findByNamedQueryAndNamedParam(
				"MediaManager.getByFileName", "fileName", media.getFileName());
		if (l != null && l.size() > 0) {
			return false;
		}

		// Passed all verifications
		return true;
	}

	@Override
	public Media createNew(Media media, byte[] content) {
		// Add a random string to file name to avoid naming conflicts
		String randomString = RandomStringUtils.random(16, true, true);
		media.setFileName(randomString + "_" + media.getFileName());

		// Do verifications.
		if (!this.verifyConstaint(media)) {
			log
					.error("Please meet below requirements and then retry: (1) fileName is unqiue, (2) title is not null");
			return null;
		}

		try {
			// Save file now
			File f = new File(media.getFileAbsolutePath());
			OutputStream outputStream = new FileOutputStream(f);
			outputStream.write(content, 0, content.length);
			outputStream.close();
		} catch (Exception ex) {
			log
					.error("Error during upload media file. Msg: "
							+ ex.getMessage());
			return null;
		}

		// Save the media now
		Media savedMedia = this.save(media);

		// Return success
		if (null != savedMedia) {
			return savedMedia;
		} else {
			log.error("Fail to save the media to db.");
			new File(media.getFileAbsolutePath()).delete();
			return null;
		}
	}

	@SuppressWarnings("unchecked")
	@Override
	public List<Media> searchByText(String text, int firstResult, int maxResult) {
		Criteria criteria = getHibernateTemplate().getSessionFactory()
				.getCurrentSession().createCriteria(Media.class);
		Criterion titleC = Restrictions.sqlRestriction("upper(title) like ('%" + text.toUpperCase() + "%')");
		Criterion fileNameC = Restrictions.sqlRestriction("upper(fileName) like ('%" + text.toUpperCase() + "%')");
		LogicalExpression orExp = Restrictions.or(fileNameC, titleC);
		criteria.add(orExp);
		if (maxResult > 0) {
			criteria.setFirstResult(firstResult);
			criteria.setMaxResults(maxResult);
		}
		return criteria.list();
	}

	@Override
	public int searchByTextCount(String text) {
		final String titleQueryString = "upper(title) like ('%"
				+ text.toUpperCase() + "%')";
		final String fileNameQueryString = "upper(fileName) like ('%"
				+ text.toUpperCase() + "%')";
		final Query qry = getHibernateTemplate().getSessionFactory()
				.getCurrentSession().createQuery(
						"select count(id) from com.beelun.shoppro.model.Media where "
								+ titleQueryString + " or "
								+ fileNameQueryString);
		Object uniqueResult = qry.uniqueResult();
		if (null == uniqueResult) {
			return 0;
		} else {
			return ((Number) uniqueResult).intValue();
		}
	}

}
