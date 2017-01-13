using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using S = System.IO;
using System.Data.SqlClient;
using System.Configuration;



using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YT {

    class Comments {

        private static YouTubeService _yts = new YouTubeService(new BaseClientService.Initializer { //to: private, .cctor or property
            ApiKey = ConfigurationManager.AppSettings["ApiKey"],
            ApplicationName = ConfigurationManager.AppSettings["ApplicationName"]
        });

        public static void Get(string id) {
            SqlConnection mc = new SqlConnection();
            mc.ConnectionString = ConfigurationManager.ConnectionStrings["cc"].ToString();

            var ctl = new List<CommentThread>();
            var cl = new List<String>();
            var al = new List<String>();

            var res = GetThread(id);
            ctl.AddRange(res.Items);

            string np = res.NextPageToken;
            while (np != null) {
                res = GetThread(id, res.NextPageToken);
                np = res.NextPageToken;
                ctl.AddRange(res.Items);
            }
            try {
                mc.Open();
                foreach (var ct in ctl) {
                    var c = ct.Snippet.TopLevelComment.Snippet.TextDisplay;
                    cl.Add(c);
                    if (c.Length >= 8000) c = c.Substring(0, 7999);
                    Console.WriteLine(c);

                    Console.Write("OPEN");

                    var s = " INSERT INTO Comments2 (Content) " +
                                       "Values (@v)";
                    SqlCommand mco = new SqlCommand(s, mc);
                    mco.Parameters.AddWithValue("@v", c);
                    mco.ExecuteNonQuery();

                    using (System.IO.StreamWriter file =
                        new System.IO.StreamWriter(@"W:\ddf\Code\YouTubeAPI\Cmd\Comments.txt", true)) {
                        file.WriteLine(c);
                    }
                    if (c.Contains("<a") && !c.Contains("ot-hashtag")) al.Add(c);
                }
            }
            finally {
                mc.SafeDispose();
                //if (mc != null) mc.Close();
                //if (mc != null) ((IDisposable)mc).Dispose();
            }
        }


        static CommentThreadListResponse GetThread(string id, string npt = "") {

            var req = _yts.CommentThreads.List("id, replies, snippet");
            req.VideoId = id;
            //req.TextFormat = CommentThreadsResource.ListRequest.TextFormatEnum.PlainText;

            if (npt != null) req.PageToken = npt;

            return req.Execute();
        }

        public static void Insert(string s) {

            var cs = new CommentSnippet() { TextDisplay = s };
            var c = new Comment() { Snippet = cs };
            var cts = new CommentThreadSnippet() {
                VideoId = "6OBY9O2ngtg",
                TopLevelComment = c
            };
            var ct = new CommentThread() { Snippet = cts };
            var req = _yts.CommentThreads.Insert(ct, "snippet");

            req.Execute();
        }
    }
}

