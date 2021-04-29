using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Text;

public class DataManager
{
    public Code code;
    public IntroData intro;
    public MainData[] mainArray = new MainData[6];
    public OutroData outro;
}
public class Code
{
    public Dictionary<int, string> codeDictionary = new Dictionary<int, string>();
}
public class IntroData
{
    public List<string> NarrationList = new List<string>();
    public List<string> Narration_Kor = new List<string>();
    public List<string> Narration_Eng = new List<string>();
}
public class MainData
{
    public int roomNo;

    public int artCount;
    public List<GameObject> artPieces = new List<GameObject>();
    public Dictionary<int, GameObject> num_artPiece;

    public List<AudioClip> doorAudio = new List<AudioClip>();

    public List<string> doorCaption = new List<string>();
    public List<string> doorCaption_Kor = new List<string>();
    public List<string> doorCaption_Eng = new List<string>();

    public List<string> artCaption = new List<string>();
    public List<string> artCaption_Kor = new List<string>();
    public List<string> artCaption_Eng = new List<string>();

    public Dictionary<GameObject, string> artPair = new Dictionary<GameObject, string>();
    public Dictionary<GameObject, string> artPair_Kor;
    public Dictionary<GameObject, string> artPair_Eng;

}
public class OutroData
{
    public Dictionary<string, FortuneResult> fortuneDictionary_KOR = new Dictionary<string, FortuneResult>();
    public Dictionary<string, FortuneResult> fortuneDictionary_ENG = new Dictionary<string, FortuneResult>();

    public List<string> NarrationList = new List<string>();
    public List<string> Narration_Kor = new List<string>();
    public List<string> Narration_Eng = new List<string>();

}
public class FortuneResult
{
    public string title;
    public string description;
    public string title_CHI;
}

public class CSVReader
{

    public static DataManager CSVtoDataManager()
    {
        DataManager dm = new DataManager();

        Code code = new Code();
        dm.code = code;

        IntroData intro = new IntroData();
        dm.intro = intro;
        OutroData outro = new OutroData();
        dm.outro = outro;

        string[,] artCaptions = CSVToMatrix("ArtCaptions");
        #region 읽었는지 확인하기 위한 출력
        //for (int i = 0; i < artCaptions.GetLength(0); i++)
        //{
        //    for (int j = 0; j < artCaptions.GetLength(1); j++)
        //    {
        //        Debug.Log("아트캡션: "+ i + "행 " + j + "열은" + artCaptions[i, j]);
        //    }
        //}
        #endregion
        FillClass(artCaptions, dm);

        string[,] doorCaptions = CSVToMatrix("NarrationCaptions");
        #region 읽었는지 확인하기 위한 출력
        //for (int i = 0; i < doorCaptions.GetLength(0); i++)
        //{
        //    for (int j = 0; j < doorCaptions.GetLength(1); j++)
        //    {
        //        Debug.Log("도어캡션: " + i + "행 " + j + "열은" + doorCaptions[i, j]);
        //    }
        //}
        #endregion
        FillClass(doorCaptions, dm);

        string[,] introNarration = CSVToMatrix("IntroNarration");
        #region 읽었는지 확인하기 위한 출력
        //for (int i = 0; i < introNarration.GetLength(0); i++)
        //{
        //    for (int j = 0; j < introNarration.GetLength(1); j++)
        //    {
        //        Debug.Log("인트로나레이션: " + i + "행 " + j + "열은" + introNarration[i, j]);
        //    }
        //}
        #endregion
        FillClass(introNarration, dm);

        string[,] randomCode = CSVToMatrix("RandomCode");
        FillClass(randomCode, dm);

        string[,] FortuneResults = CSVToMatrix("64FortuneResult");
        FillClass(FortuneResults, dm);

        string[,] outroNarration = CSVToMatrix("OutroNarration");
        #region 읽었는지 확인하기 위한 출력
        //for (int i = 0; i < outroNarration.GetLength(0); i++)
        //{
        //    for (int j = 0; j < outroNarration.GetLength(1); j++)
        //    {
        //        Debug.Log("아웃트로나레이션: " + i + "행 " + j + "열은" + outroNarration[i, j]);
        //    }
        //}
        #endregion
        FillClass(outroNarration, dm);

        return dm;

    }

    public static string[,] CSVToMatrix(string file)
    {
        string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
        string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
        char[] TRIM_CHARS = { '\"' };

        TextAsset csvData = Resources.Load(file) as TextAsset; 
        csvData.text.Replace('"', ' '); 
        string[] lines = Regex.Split(csvData.text, LINE_SPLIT_RE);
        if (lines.Length <= 1) 
        {
            Debug.LogError("데이터에 내용이 없습니다");
            return null;
        }
        string[] header = Regex.Split(lines[0], SPLIT_RE); 
        string[,] csvMat = new string[lines.Length, header.Length]; 
        for (int i = 0; i < lines.Length; i++)
        {
            
            string[] values = Regex.Split(lines[i], SPLIT_RE); 
            for (int j = 0; j < header.Length; j++)
            {
                string temp= RemoveQuotation(values[j]);
                temp= DollarToEnter(temp);
                temp = AtToQuotation(temp);
                csvMat[i, j] = temp;
            }
        }
        return csvMat;
    }
    public static string RemoveQuotation(string str)
    {
        StringBuilder sb = new StringBuilder();
        string answer; 
        if (str.Contains('"'))
        {
            for (int l = 0; l < str.Length; l++)
            {
                if (str[l] != '"')
                    sb.Append(str[l]);
            }
            answer = sb.ToString();
        }
        else
            answer = str;
        return answer;
    }

    public static string DollarToEnter(string str)
    {
        StringBuilder sb = new StringBuilder();
        string answer;
        if (str.Contains('$'))
        {
            for (int l = 0; l < str.Length; l++)
            {
                if (str[l] != '$')
                    sb.Append(str[l]);
                else
                    sb.AppendLine();
            }
            answer = sb.ToString();
        }
        else
            answer = str;
        return answer;
    }
    public static string AtToQuotation(string str)
    {
        StringBuilder sb = new StringBuilder();
        string answer;
        if (str.Contains('@'))
        {
            for (int l = 0; l < str.Length; l++)
            {
                if (str[l] != '@')
                    sb.Append(str[l]);
                else
                    sb.Append('"');
            }
            answer = sb.ToString();
        }
        else
            answer = str;
        return answer;
    }

    public static void FillClass(string[,] matrix, DataManager dataManager)
    {
        if (matrix[1, 0] == "Code")
        {
            MatToCode(matrix, dataManager);
        }

        else if (matrix[1, 0] == "Intro")
        {
            MatToIntro(matrix, dataManager);
        }

        else if (matrix[1, 0] == "Main") 
        {
            int col = matrix.GetLength(1);
            List<string> header = new List<string>();
            for (int i = 0; i < col; i++)
            {
                header.Add(matrix[0, i]);
            }
            if (header.Contains("ObjectNum"))
                ArtCaptions(matrix, dataManager);
            else if (header.Contains("Type"))
                DoorCaptions(matrix, dataManager);
        }
        else if (matrix[1, 0] == "Outro")
        {
            int col = matrix.GetLength(1);
            List<string> header = new List<string>();
            for (int i = 0; i < col; i++)
            {
                header.Add(matrix[0, i]);
            }
            
            if (header.Contains("binaryCode"))
            {
                MatToOutro(matrix, dataManager);
            }
            else
            {
                MatToOutroNarration(matrix, dataManager);
            }
        }
        else
        {
            Debug.Log("데이터의 stage를 알 수 없음");
        }
    }
    public static void ArtCaptions(string[,] matrix, DataManager dataManager)
    {
        for (int i = 1; i < matrix.GetLength(0); i++)
        {
            for (int j = 1; j <= 6; j++)
            {
                if (Convert.ToInt32(matrix[i, 1]) == j)
                {
                    if (dataManager.mainArray[j - 1] == null)
                    {
                        dataManager.mainArray[j - 1] = new MainData();
                    }
                    dataManager.mainArray[j - 1].roomNo = j;
                    dataManager.mainArray[j - 1].artCaption_Kor.Add(matrix[i, 3]);
                    dataManager.mainArray[j - 1].artCaption_Eng.Add(matrix[i, 4]);
                    dataManager.mainArray[j - 1].artCount = Convert.ToInt32(matrix[i, 2]);
                }
            }
        }
    }

    public static void DoorCaptions(string[,] matrix, DataManager dataManager)
    {
        for (int i = 1; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < 6; j++) 
            {
                if (Convert.ToInt32(matrix[i, 1]) == j + 1)
                {
                    dataManager.mainArray[j].doorCaption_Kor.Add(matrix[i, 4]);
                    dataManager.mainArray[j].doorCaption_Eng.Add(matrix[i, 5]);
                }
            }
        }
    }
    public static void MatToCode(string[,] matrix, DataManager dataManager)
    {

        for (int i = 1; i < matrix.GetLength(0); i++)
        {
            if (matrix[i, 2] != "")
            {
                dataManager.code.codeDictionary.Add(int.Parse(matrix[i, 1]), matrix[i, 2]);
            }
        }
    }
    public static void MatToIntro(string[,] matrix, DataManager dataManager)
    {
        dataManager.intro.Narration_Kor.Add(matrix[1, 2]);
        dataManager.intro.Narration_Kor.Add(matrix[2, 2]);
        dataManager.intro.Narration_Kor.Add(matrix[3, 2]);
        dataManager.intro.Narration_Kor.Add(matrix[4, 2] + "\n" + matrix[5, 2]);
        dataManager.intro.Narration_Kor.Add(matrix[6, 2]);
        dataManager.intro.Narration_Kor.Add(matrix[7, 2] + "\n" + matrix[8, 2]);
        dataManager.intro.Narration_Kor.Add(matrix[9, 2] + "\n" + matrix[10, 2]);
        dataManager.intro.Narration_Kor.Add(matrix[11, 2]);
        dataManager.intro.Narration_Kor.Add(matrix[14, 2]);

        dataManager.intro.Narration_Kor.Add(matrix[15, 2] + "\n" + matrix[16, 2]);
        dataManager.intro.Narration_Kor.Add(matrix[15, 2] + "\n" + matrix[16, 2]);

        dataManager.intro.Narration_Kor.Add(matrix[18, 2] + "\n" + matrix[19, 2]);
        dataManager.intro.Narration_Kor.Add(matrix[20, 2]);
        dataManager.intro.Narration_Kor.Add(matrix[21, 2] + "\n" + matrix[22, 2]);

        dataManager.intro.Narration_Kor.Add(matrix[23, 2] + "\n" + matrix[24, 2]);
        dataManager.intro.Narration_Kor.Add(matrix[25, 2] + "\n" + matrix[26, 2]);
        dataManager.intro.Narration_Kor.Add(matrix[27, 2]);
        dataManager.intro.Narration_Kor.Add(matrix[28, 2] + "\n" + matrix[29, 2]);
        dataManager.intro.Narration_Kor.Add(matrix[30, 2] + "\n" + matrix[31, 2]);
        dataManager.intro.Narration_Kor.Add(matrix[32, 2]);
        dataManager.intro.Narration_Kor.Add(matrix[33, 2]);
        dataManager.intro.Narration_Kor.Add(matrix[34, 2]);
        dataManager.intro.Narration_Kor.Add(matrix[35, 2]);
        dataManager.intro.Narration_Kor.Add(matrix[36, 2]);
        dataManager.intro.Narration_Kor.Add(matrix[30, 2]);
        //for (int i = 0; i < dataManager.intro.Narration_Kor.Count; i++)
        //    Debug.Log("한글 " + dataManager.intro.Narration_Kor[i]);

        dataManager.intro.Narration_Eng.Add(matrix[1, 3] + "\n" + matrix[2, 3]);
        dataManager.intro.Narration_Eng.Add(matrix[3, 3] + "\n" + matrix[4, 3]);
        dataManager.intro.Narration_Eng.Add(matrix[3, 3] + "\n" + matrix[4, 3]);
        dataManager.intro.Narration_Eng.Add(matrix[5, 3] + "\n" + matrix[6, 3]);
        dataManager.intro.Narration_Eng.Add(matrix[7, 3]);
        dataManager.intro.Narration_Eng.Add(matrix[8, 3] + "\n"+ matrix[9, 3]);
        dataManager.intro.Narration_Eng.Add(matrix[10, 3] + "\n" + matrix[11, 3]+"\n" + matrix[12, 3]);
        dataManager.intro.Narration_Eng.Add(matrix[13, 3]);
        dataManager.intro.Narration_Eng.Add(matrix[14, 3]); 
        dataManager.intro.Narration_Eng.Add(matrix[15, 3] + "\n" + matrix[16, 3]);
        dataManager.intro.Narration_Eng.Add(matrix[17, 3]);
        dataManager.intro.Narration_Eng.Add(matrix[18, 3] + "\n" + matrix[19, 3]);
        dataManager.intro.Narration_Eng.Add(matrix[20,3]); 
        dataManager.intro.Narration_Eng.Add(matrix[21, 3] + "\n" + matrix[22, 3]); 
        dataManager.intro.Narration_Eng.Add(matrix[23, 3] + "\n" + matrix[24, 3]);
        dataManager.intro.Narration_Eng.Add(matrix[25, 3] + "\n" + matrix[26, 3]);
        dataManager.intro.Narration_Eng.Add(matrix[27, 3] + "\n" + matrix[28, 3]);
        dataManager.intro.Narration_Eng.Add(matrix[29, 3] + "\n" + matrix[30, 3] + "\n" + matrix[31, 3]);
        dataManager.intro.Narration_Eng.Add(matrix[32, 3] + "\n" + matrix[33, 3]);
        dataManager.intro.Narration_Eng.Add(matrix[34, 3]);
        dataManager.intro.Narration_Eng.Add(matrix[35, 3]);
        dataManager.intro.Narration_Eng.Add(matrix[36, 3]);
    }

    public static void MatToOutro(string[,] matrix, DataManager dataManager)
    {

        for (int i = 1; i < matrix.GetLength(0); i++)
        {
            if (matrix[i, 0] != "")
            {
                FortuneResult fortuneResult_KOR = new FortuneResult();
                fortuneResult_KOR.title = matrix[i, 3];
                fortuneResult_KOR.title_CHI = matrix[i, 4];
                fortuneResult_KOR.description = matrix[i, 6];

                FortuneResult fortuneResult_ENG = new FortuneResult();
                fortuneResult_ENG.title = matrix[i, 5];
                fortuneResult_ENG.title_CHI = matrix[i, 4];
                fortuneResult_ENG.description = matrix[i, 7];

                
                dataManager.outro.fortuneDictionary_KOR.Add(matrix[i, 2], fortuneResult_KOR);
                dataManager.outro.fortuneDictionary_ENG.Add(matrix[i, 2], fortuneResult_ENG);
            }
        }
    }

    private static void MatToOutroNarration(string[,] matrix, DataManager dataManager)
    {
        for (int i = 1; i < matrix.GetLength(0); i++)
        {
                dataManager.outro.Narration_Kor.Add(matrix[i, 2]);
                dataManager.outro.Narration_Eng.Add(matrix[i, 3]);
        }
    }
}


