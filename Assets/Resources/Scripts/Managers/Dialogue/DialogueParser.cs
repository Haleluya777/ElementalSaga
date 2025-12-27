using System.Collections.Generic;
using UnityEngine;

//JSON test
[System.Serializable]
public class DataWarpper
{
    public string Detail;
    public string Speaker;
}

[System.Serializable]
public class DialogueDatas
{
    public List<DataWarpper> TestedSheet;
}
//---------------------------------------
public class DialogueParser : MonoBehaviour
{
    public struct ParsedLine
    {
        public string Actor;
        public string Detail;
        public int LineNum;
    }

    public List<ParsedLine> Parse(string csvFile)
    {
        List<ParsedLine> parsedLines = new List<ParsedLine>();
        string[] lines = csvFile.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue;

            List<string> parts = new List<string>(line.Split('	'));
            //Debug.Log(parts.Count);

            while (parts.Count < 2)
            {
                parts.Add("");
            }
            //parts.RemoveAll(s => string.IsNullOrEmpty(s));

            for (int j = 0; j < parts.Count; j++)
            {
                parts[j] = parts[j].Trim();
                //Debug.Log(parts[j]);
            }

            ParsedLine parsedLine = new ParsedLine
            {
                Actor = parts[0],
                Detail = parts[1],
                LineNum = i
            };

            parsedLines.Add(parsedLine);
        }
        return parsedLines;
    }
}
