using System;
using System.Text;
using System.Collections.Generic;

namespace HappyDog.SL.Data
{
    /// <summary>
    /// where clause builder
    /// Ideally, we need something like Product Studio query builder.
    /// We are talking to Hibernate in the backend. Refer to DT24
    /// 
    /// HB docs:
    /// http://docs.jboss.org/hibernate/stable/core/reference/en/html/queryhql-where.html
    /// http://docs.jboss.org/hibernate/stable/core/reference/en/html/queryhql-expressions.html
    /// 
    /// Current versio is a v0.01 only
    /// </summary>
    public class WhereClauseBuilder
    {
        private class KeyValuePair
        {
            public string Key {get;set;}
            public string Value { get; set; }
        }
        List<KeyValuePair> container = new List<KeyValuePair>();

        public WhereClauseBuilder() { }

        /// <summary>
        /// Add a key/value pair to the whare clause
        /// If the key already exists, we will overwrite.
        /// 
        /// Notes: If the property is string, you have to pass in 'value' instead of value.
        /// At this moment, this problem is left to class consumer to handle
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, string value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                return;
            }
            KeyValuePair kvp = new KeyValuePair();
            kvp.Key = key;
            kvp.Value = value;
            container.Add(kvp);
        }

        /// <summary>
        /// Return the current where clause
        /// It should return something like:
        ///     ?where=(currency='102' AND iBANLength=23)
        /// </summary>
        /// <returns></returns>
        public string GetString(WhereClauseBuilderOperatorEnum op)
        {
            if (container == null || container.Count == 0)
            {
                return null;
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(@"?where=(");
                bool bFirstOne = true;
                foreach (KeyValuePair kvp in container)
                {
                    if (bFirstOne)
                    {
                        bFirstOne = false;
                    }
                    else
                    {
                        sb.Append(string.Format(@" {0} ", op.ToString()));
                    }
                    sb.Append(string.Format("{0}={1}", kvp.Key, kvp.Value));
                }
                sb.Append(@")");

                return sb.ToString();
            }
        }
    }

    /// <summary>
    /// And/Or 
    /// </summary>
    public enum WhereClauseBuilderOperatorEnum
    { 
        AND = 0,
        OR 
    }
}
