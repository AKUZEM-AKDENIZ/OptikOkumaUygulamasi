using System.Text;
using System.Text.RegularExpressions;

namespace ExamScoreCalculator
{
    static class Program
    {
        private static string ROOT_DIR = "./Files";
        private static string ANSWER_KEY_DIR;
        private static string OUTPUT_DIR;
        private static string OUTPUT_DELIMETER = ",";
        private static string EXAMS_DIR;
        private static string ERRORS_DIR;
        private static StringBuilder sbError = new StringBuilder();

        public static void Main(string[] args)
        {
            /*
            // Dosya yollarını tanımla
            string path1 = @"./Sonuc.csv";
            string path2 = @"./SonucEski.csv";
            string outputPath = @"./SonSonuc.csv";

            // Her iki dosyayı da satır satır oku
            var file1Lines = File.ReadAllLines(path1, Encoding.UTF8);
            var file2Lines = File.ReadAllLines(path2, Encoding.UTF8);
           

            List<string> differences = new List<string>();

            int maxLength = Math.Min(file1Lines.Length, file2Lines.Length);
            for (int i = 0; i < maxLength; i++)
            {
                var line1 = file1Lines[i];
                var line2 = file2Lines[i];

                if (line1 != line2)
                {
                    var columns1 = line1.Split(',');
                    var columns2 = line2.Split(',');

                    string differenceLine = columns1[0]; // Assuming the first column is a unique identifier
                    string differencesDetail = "";

                    for (int j = 1; j < 4; j++)
                    {
                        if (columns1[j] != columns2[j])
                        {
                            int value1 = int.TryParse(columns1[j], out value1) ? value1 : 0;
                            int value2 = int.TryParse(columns2[j], out value2) ? value2 : 0;

                            int diff = value1 - value2;
                            differenceLine += "," + columns1[j];
                            differencesDetail += "," + (diff > 0 ? "+" : "") + diff.ToString();
                        }
                        else
                        {
                            differenceLine += "," + columns1[j];
                            differencesDetail += ",=";
                        }
                    }

                    differences.Add(differenceLine + differencesDetail + "," + columns1[4]);
                }
            }

            File.WriteAllLines(outputPath, differences, Encoding.UTF8);
            Console.WriteLine("Differences written to " + outputPath); 


            */

            
             try
             {
                 //string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                 //ROOT_DIR = System.IO.Path.GetDirectoryName(exePath);
                 Console.WriteLine(ROOT_DIR);

                 string answerKeyFileName;
                 do
                 {
                     Console.WriteLine("Cevap anahtarı dosyasının ismini giriniz(Ör:cevapFinal.txt):");
                     //answerKeyFileName = Console.ReadLine();
                     //ANSWER_KEY_DIR = $"{ROOT_DIR}/cevap/{answerKeyFileName}";
                     ANSWER_KEY_DIR = $"{ROOT_DIR}/cevap/cevap.txt";
                     if (!File.Exists(ANSWER_KEY_DIR))
                     {
                         Console.WriteLine("Dosya yok. Lütfen geçerli bir dosya adı girin.");
                     }
                 } while (!File.Exists(ANSWER_KEY_DIR));
                 string outputFolder;
                 do
                 {
                     Console.WriteLine("Sonuçların kaydedileceği klasörün ismini giriniz:");
                     //outputFolder = Console.ReadLine();
                     //OUTPUT_DIR = $"{ROOT_DIR}/{outputFolder}/";
                     OUTPUT_DIR = $"{ROOT_DIR}/cevap/";
                     if (!Directory.Exists(OUTPUT_DIR))
                     {
                         Console.WriteLine("Klasör mevcut değil. Lütfen geçerli bir klasör adı girin.");
                     }
                 } while (!Directory.Exists(OUTPUT_DIR));
                 string examFolder;
                 do
                 {
                     Console.WriteLine("Sınavların bulunduğu klasörün ismini giriniz:");
                     //examFolder = Console.ReadLine();
                     //EXAMS_DIR = $"{ROOT_DIR}/{examFolder}/";
                     EXAMS_DIR = $"{ROOT_DIR}/sinav/";
                     if (!Directory.Exists(EXAMS_DIR))
                     {
                         Console.WriteLine("Klasör mevcut değil. Lütfen geçerli bir klasör adı girin.");
                     }
                 } while (!Directory.Exists(EXAMS_DIR));
                 //ERRORS_DIR = $"{ROOT_DIR}/{outputFolder}/";
                 ERRORS_DIR = $"{ROOT_DIR}/cevap/";

                 ReadStudentAnswerWithOneFile();


             }
             catch (Exception e)
             {
                 Console.WriteLine("Bilinmeyen bir hata oldu lütfen geliştirici ile ilgili temasta bulununuz.");
                 Console.WriteLine(e.Message);
             }
             finally
             {
                 Console.WriteLine("Çıkmak için bir tuşa basın.");
                 Console.ReadLine();
             }
        }


        public static void ReadStudentAnswerWithOneFile()
        {
            string[] files = Directory.GetFiles(EXAMS_DIR);
            StringBuilder sb = new StringBuilder();
            foreach (var file in files)
            {
                if (Path.GetExtension(file) == ".txt")
                {
                    string facultyAnswers = ReadStudentAnswersByFaculty(file, Path.GetFileNameWithoutExtension(file));
                    sb.Append(facultyAnswers);
                }
            }
            WriteStudentAnswersByFaculty(sb.ToString(), OUTPUT_DIR + "Sonuc.csv");
            WriteErrorsByFaculty(sbError.ToString(), ERRORS_DIR + "HatalıOkunanlar.csv");
        }

        private static void WriteStudentAnswersByFaculty(string studentScore, string fileName)
        {
            try
            {
                Console.WriteLine(fileName);
                System.IO.File.WriteAllText(fileName, studentScore, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void WriteErrorsByFaculty(string errorFile, string fileName)
        {
            try
            {
                System.IO.File.WriteAllText(fileName, errorFile, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static string ReadStudentAnswersByFaculty(string filePath, string fileName)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                var lines = System.IO.File.ReadAllLines(filePath);
                var answerKeys = ReadAnswerKeys(ANSWER_KEY_DIR);
                foreach (var line in lines)
                {
                    var data = line;
                    if (data.Length < 83)
                    {
                        continue;
                    }
                    string[] parts = new string[5];

                    parts[0] = data.Substring(0, 12);
                    data = data.Substring(12);


                    string studentId = parts[0].Trim();

                    if (studentId.Trim().Length == 0)
                    {
                        continue;
                    }
                    string lineFromOther = line.Substring(83);
                    bool containsLetters = lineFromOther.Any(c => c == 'A' || c == 'B' || c == 'C' || c == 'D' || c == 'E');
                    if (studentId.Contains("*") || studentId.Contains(" ") || studentId.Length < 11 || line.Contains("*") || containsLetters)
                    {
                        sbError.Append(studentId);
                        sbError.Append(OUTPUT_DELIMETER);
                        sbError.Append(fileName);
                        sbError.Append(Environment.NewLine);
                    }

                    parts[1] = data.ElementAt(0).ToString(); // Kitapçık türü
                    data = data.Substring(1);
                    string[] studentAnswer = new string[3];
                    int index = 0;
                    for (int i = 2; i < 5 && data.Length >= 25; i++)
                    {
                        parts[i] = data.Substring(0, 20); 
                        data = data.Substring(25);
                        studentAnswer[index++] = parts[i];
                    }


                    int[] scores = CalculateStudentScore(studentAnswer, parts[1].ToCharArray(), answerKeys);
                    sb.Append(studentId);
                    sb.Append(OUTPUT_DELIMETER);
                    for (int i = 0; i < scores.Length; i++)
                    {
                        sb.Append(scores[i]);
                        sb.Append(OUTPUT_DELIMETER);
                    }
                    sb.Append(fileName);
                    sb.Append(Environment.NewLine);
                }
                return sb.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private static int[] CalculateStudentScore(string[] studentAnswer, char[] bookletType, Dictionary<string, string[]> answerKey)
        {
            
            string examType = new string(bookletType).Trim();
            string[] answerKeys;
            answerKey.TryGetValue(examType, out answerKeys);
            int[] scores = new int[3];
            if (answerKeys != null)
            {
                for (int i = 0; i < studentAnswer?.Length; i++)
                {
                    int score = 0;
                    for (int j = 0; j < studentAnswer[i]?.Length; j++)
                    {
                        if (studentAnswer[i][j] == answerKeys[i][j])
                        {
                            score++;
                        }
                    }
                    scores[i] = score;
                }
            }
            else
            {
                int maxSum = 0;
                foreach (KeyValuePair<string, string[]> set in answerKey)
                {
                    int[] temp = new int[3];
                    int sum = 0;
                    string[] answerKeysByType;
                    answerKey.TryGetValue(set.Key, out answerKeysByType);
                    for (int i = 0; i < studentAnswer.Length; i++)
                    {
                        int score = 0;
                        for (int j = 0; j < studentAnswer[i]?.Length; j++)
                        {
                            if (studentAnswer[i][j] == answerKeysByType[i][j])
                            {
                                score++;
                            }
                        }
                        temp[i] = score;
                    }
                    for (int i = 0; i < scores.Length; i++)
                    {
                        sum += temp[i];
                    }
                    if (sum > maxSum)
                    {
                        maxSum = sum;
                        scores = temp;
                    }
                }
            }
            return scores;
        }

        private static Dictionary<string, string[]> ReadAnswerKeys(string filePath)
        {
            try
            {
                var answerKeys = new Dictionary<string, string[]>();
                var lines = System.IO.File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var answerKey = Regex.Matches(line, ".{1,20}")
                                          .Cast<Match>()
                                          .Select(m => m.Value)
                                          .ToArray();
                    answerKeys.Add(answerKey[answerKey.Length - 1].Trim(), answerKey);
                }
                return answerKeys;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

    }
}
