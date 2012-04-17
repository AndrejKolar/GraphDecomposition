using System.Collections.Generic;
using System.IO;
using GraphDecomposition.GraphElements;

namespace GraphDecomposition.Utils
{
    public static class LogUtils
    {
        /// <summary>
        /// Path of the log file
        /// </summary>
        private static string path;

        /// <summary>
        /// Creates a log file with a single incidence matrix
        /// Log file starts with v and b
        /// UNIX line endings
        /// </summary>
        /// <param name="logPath">Path of the log file</param>
        public static void CreateLogFile(string logPath, SteinerTripleSystem sts)
        {
            path = logPath;

            using (StreamWriter streamWriter = new StreamWriter(path, false))
            {
                //Write v and b to the first line of the logfile
                streamWriter.Write(sts.NumVertex().ToString() +" " + sts.NumTriples().ToString() + "\n");
                streamWriter.Write("\n");
            }

            AppendIncidenceMatrix(sts);
        }

        /// <summary>
        /// Adds an incidence matrix for the STS(v) to the log file. Logfile has to be created beforehand.
        /// Rows of the matrix represent vertices and the colums represent the triples.
        /// If a triple contains an vertex, the value of the coresponding martix element is 1. Otherwise it's 0.
        /// </summary>
        /// <param name="sts"></param>
        public static void AppendIncidenceMatrix(SteinerTripleSystem sts)
        {
            using (StreamWriter streamWriter = new StreamWriter(path, true))
            {
                List<string> matrixRows = new List<string>();
                for (int i = 1; i <= sts.NumVertex(); i++)
                {
                    matrixRows.Add(string.Empty);
                }

                foreach (Triple tr in sts)
                {
                    for (int i = 1; i <= sts.NumVertex(); i++)
                    {
                        if (tr.X == i || tr.Y == i || tr.Z == i)
                        {
                            matrixRows[i - 1] += "1 ";
                        }
                        else
                        {
                            matrixRows[i - 1] += "0 ";
                        }
                    }
                }

                foreach (string line in matrixRows)
                {
                    streamWriter.Write(line.Trim() + "\n");
                }

                streamWriter.Write("\n");
            }
        }
    }
}