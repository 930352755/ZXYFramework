using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Game
{
    public static class ToolsEditor
    {
        /// <summary>
        /// 写入枚举文件
        /// </summary>
        public static void SetEnum(string enumFilePath, List<string> enumList, string enumName, string nameSpace = null)
        {
            StringBuilder areaString;
            if (nameSpace != null)
            {
                areaString = new StringBuilder("namespace " + nameSpace + "\n{\n\tpublic enum " + enumName + "\n\t{");
                string endContent = "\n\t}\n}";

                int count = enumList.Count;
                for (int i = 0; i < count; i++)
                {
                    areaString.Append("\n\t\t" + enumList[i] + ",");
                }

                areaString.Append("\n\t\t" + enumName + "Num");
                areaString.Append(endContent);
            }
            else
            {
                areaString = new StringBuilder("public enum " + enumName + "\n{");
                string endContent = "\n}";

                int count = enumList.Count;
                for (int i = 0; i < count; i++)
                {
                    areaString.Append("\n\t" + enumList[i] + ",");
                }
                areaString.Append("\n\t" + enumName + "Num");
                areaString.Append(endContent);
            }
            File.WriteAllText(enumFilePath, areaString.ToString());
        }
    }
}