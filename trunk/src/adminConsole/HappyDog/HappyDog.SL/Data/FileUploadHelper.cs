using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HappyDog.SL.Data
{
    /// <summary>
    /// Below code is borrowed from:
    /// http://killustar.blogspot.com/2009/03/silverlight-multi-part-form-post.html
    /// A much powerful one:
    /// http://www.codeplex.com/Wikipage?ProjectName=SilverlightFileUpld
    /// </summary>
    public static class Extensions
    {
        public static void PostFormAsync(this HttpWebRequest request, object parameters, AsyncCallback callback)
        {
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.BeginGetRequestStream(new AsyncCallback(asyncResult =>
            {
                Stream stream = request.EndGetRequestStream(asyncResult);
                DataContractQueryStringSerializer ser = new DataContractQueryStringSerializer();
                ser.WriteObject(stream, parameters);
                stream.Close();
                request.BeginGetResponse(callback, request);
            }), request);
        }

        public static void PostMultiPartAsync(this HttpWebRequest request, object parameters, AsyncCallback callback)
        {
            request.Method = "POST";
            string boundary = "---------------" + DateTime.Now.Ticks.ToString();
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.BeginGetRequestStream(new AsyncCallback(asyncResult =>
            {
                Stream stream = request.EndGetRequestStream(asyncResult);

                DataContractMultiPartSerializer ser = new DataContractMultiPartSerializer(boundary);
                ser.WriteObject(stream, parameters);
                stream.Close();
                request.BeginGetResponse(callback, request);
            }), request);
        }
    }

    public class DataContractQueryStringSerializer
    {
        public void WriteObject(Stream stream, object data)
        {
            StreamWriter writer = new StreamWriter(stream);
            if (data != null)
            {
                if (data is Dictionary<string, string>)
                {
                    foreach (var entry in data as Dictionary<string, string>)
                    {
                        writer.Write("{0}={1}&", entry.Key, entry.Value);
                    }
                }
                else
                {
                    foreach (var prop in data.GetType().GetFields())
                    {
                        foreach (var attribute in prop.GetCustomAttributes(true))
                        {
                            if (attribute is DataMemberAttribute)
                            {
                                DataMemberAttribute member = attribute as DataMemberAttribute;
                                writer.Write("{0}={1}&", member.Name ?? prop.Name, prop.GetValue(data));
                            }
                        }
                    }
                    foreach (var prop in data.GetType().GetProperties())
                    {
                        if (prop.CanRead)
                        {
                            foreach (var attribute in prop.GetCustomAttributes(true))
                            {
                                if (attribute is DataMemberAttribute)
                                {
                                    DataMemberAttribute member = attribute as DataMemberAttribute;
                                    writer.Write("{0}={1}&", member.Name ?? prop.Name, prop.GetValue(data, null));
                                }
                            }
                        }
                    }
                }
                writer.Flush();
            }
        }
    }

    public class DataContractMultiPartSerializer
    {
        private string boundary;
        public DataContractMultiPartSerializer(string boundary)
        {
            this.boundary = boundary;
        }

        private void WriteEntry(StreamWriter writer, string key, object value)
        {
            if (value != null)
            {
                writer.Write("--");
                writer.WriteLine(boundary);
                if (value is FileInfo)
                {

                    FileInfo f = value as FileInfo;
                    writer.WriteLine(@"Content-Disposition: form-data; name=""{0}""; filename=""{1}""", key, f.Name);
                    writer.WriteLine("Content-Type: application/octet-stream");
                    writer.WriteLine("Content-Length: " + f.Length);
                    writer.WriteLine();
                    writer.Flush();
                    Stream output = writer.BaseStream;
                    Stream input = f.OpenRead();
                    byte[] buffer = new byte[4096];
                    for (int size = input.Read(buffer, 0, buffer.Length); size > 0; size = input.Read(buffer, 0, buffer.Length))
                    {
                        output.Write(buffer, 0, size);
                    }
                    output.Flush();
                    writer.WriteLine();
                }
                else
                {
                    writer.WriteLine(@"Content-Disposition: form-data; name=""{0}""", key);
                    writer.WriteLine();
                    writer.WriteLine(value.ToString());
                }
            }
        }

        public void WriteObject(Stream stream, object data)
        {
            StreamWriter writer = new StreamWriter(stream);
            if (data != null)
            {
                if (data is Dictionary<string, object>)
                {
                    foreach (var entry in data as Dictionary<string, object>)
                    {
                        WriteEntry(writer, entry.Key, entry.Value);
                    }
                }
                else
                {
                    foreach (var prop in data.GetType().GetFields())
                    {
                        foreach (var attribute in prop.GetCustomAttributes(true))
                        {
                            if (attribute is DataMemberAttribute)
                            {
                                DataMemberAttribute member = attribute as DataMemberAttribute;
                                WriteEntry(writer, member.Name ?? prop.Name, prop.GetValue(data));
                            }
                        }
                    }
                    foreach (var prop in data.GetType().GetProperties())
                    {
                        if (prop.CanRead)
                        {
                            foreach (var attribute in prop.GetCustomAttributes(true))
                            {
                                if (attribute is DataMemberAttribute)
                                {
                                    DataMemberAttribute member = attribute as DataMemberAttribute;
                                    WriteEntry(writer, member.Name ?? prop.Name, prop.GetValue(data, null));
                                }
                            }
                        }
                    }
                }
            }
            writer.Write("--");
            writer.Write(boundary);
            writer.WriteLine("--");
            writer.Flush();
        }
    }
}
