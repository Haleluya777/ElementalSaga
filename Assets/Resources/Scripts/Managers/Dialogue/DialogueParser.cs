using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public struct ParsedLine
    {
        public string Action;
        public Detail Detail;
        public string BG;
        public string Face;
        public string Actor;
        public string BGM;
        public string SE;
        //public string CutScene;
        public int LineNum;
    }

    public struct Detail
    {
        public string condition;
        public string result;
    }

    public List<ParsedLine> Parse(string tsvFile)
    {
        //JSon test
        //DialogueDatas testline = JsonUtility.FromJson<DialogueDatas>(csvFile);
        //Debug.Log(testline.TestedSheet.Count);

        List<ParsedLine> parsedLines = new List<ParsedLine>(); //파싱한 줄을 보관할 리스트
        string[] lines = tsvFile.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries); //줄바꿈을 기준으로 한 줄을 분리.

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue;

            List<string> parts = new List<string>(line.Split('	'));
            //Debug.Log(parts.Count);

            while (parts.Count < 11)
            {
                parts.Add("");
            }
            //parts.RemoveAll(s => string.IsNullOrEmpty(s));

            for (int j = 0; j < parts.Count; j++)
            {
                parts[j] = parts[j].Trim();
                //Debug.Log(parts[j]);
            }

            string action = parts[0]; //첫번째 열(행동)을 action변수에 저장.
            string _condition = "";
            string _result = "";

            if (parts[1].Contains('_'))
            {
                _condition = parts[1].Split('_')[0]; //두번째 열을 '_'로 나누어 앞부분을 _condition변수에 저장.
                _result = parts[1].Split('_')[1]; //두번째 열을 '_'로 나누어 뒷부분을 _result변수에 저장.
            }
            else
            {
                _condition = parts[1]; //두번째 열에 '_'가 없을 경우, _condition은 빈 문자열로.
                _result = ""; //나머지 모든 내용은 _result에.
                //Debug.Log(_result);
            }

            Detail detail = new Detail
            {
                condition = _condition,
                result = _result
            };

            ParsedLine parsedLine = new ParsedLine
            {
                Action = action,
                Detail = detail,
                BG = parts[2],
                Face = parts[3],
                Actor = parts[4],
                BGM = parts[5],
                SE = parts[6],
                //CutScene = parts[7],
                LineNum = i
            };

            parsedLines.Add(parsedLine);
        }
        // for (int i = 0; i < parsedLines.Count; i++)
        // {
        //     Debug.Log($"라인 {i} : Action={parsedLines[i].Action}, Condition={parsedLines[i].Detail.condition}, Result={parsedLines[i].Detail.result}, BG={parsedLines[i].BG}, Face={parsedLines[i].Face}, Actor={parsedLines[i].Actor}, BGM={parsedLines[i].BGM}");
        // }
        return parsedLines;
    }
}
