using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using S = System.IO;


using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YT {

    class Comments {

        public static YouTubeService yts = new YouTubeService(new BaseClientService.Initializer {
            ApiKey = System.Configuration.ConfigurationManager.AppSettings["ApiKey"], 
            ApplicationName = "youtube-comment-155118"
        });

        public static void Get(string id) {

            //var Exec = System.Configuration.ConfigurationManager.AppSettings["Dir.Exec"];

            //StreamWriter _writer;
            //_writer = S.File.CreateText(Exec + "Comments.txt");
            //_writer.AutoFlush = true;
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
            foreach (var ct in ctl) {
                var c = ct.Snippet.TopLevelComment.Snippet.TextDisplay;
                cl.Add(c);
                Console.WriteLine(c);
                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(@"W:\ddf\Code\YouTubeAPI\Cmd\Comments.txt", true)) {
                    file.WriteLine(c);
                }
                if (c.Contains("<a") && !c.Contains("ot-hashtag")) al.Add(c);
            }
            return;
        }

        static CommentThreadListResponse GetThread(string id, string npt = "") {

            var req = yts.CommentThreads.List("id, replies, snippet");
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
            var req = yts.CommentThreads.Insert(ct, "snippet");

            req.Execute();
        }

    }
}
