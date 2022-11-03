using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowBase
{
    private Dictionary<int, Dictionary<string, string>> table = new Dictionary<int, Dictionary<string, string>>();

    public void Load(string text)
    {
        string[] rows = text.Split("\n");

        int length = rows.Length -1 ;
        if (string.IsNullOrEmpty(rows[length])) length--;

        for(int i = 0 ; i < length ; i++)
        {
            rows[i] = rows[i].Replace("\r", "");
        }

        string[] subjects = rows[0].Split(",");

        for(int r = 1; r <= length ; r++)
        {
            string[] columns = rows[r].Split(",");
            int tableID = -1;
            int.TryParse(columns[0], out tableID);

            if(table.ContainsKey(tableID) == false)
                table.Add(tableID, new Dictionary<string, string>());

            for(int c = 1 ; c < columns.Length ; c++)
            {
                if(table[tableID].ContainsKey(subjects[c]) == false)
                    table[tableID].Add(subjects[c], columns[c]);
            }
        }
    }

    public string ToS(int tableID, string subkey)
    {
        string str = string.Empty;
        if(table.ContainsKey(tableID))
        {
            if(table[tableID].ContainsKey(subkey))
                str = table[tableID][subkey];
        }
        return str;
    }

    public int ToI(int tableID, string subKey)
    {
        int val = -1;
        if (table.ContainsKey(tableID))
        {
            if (table[tableID].ContainsKey(subKey))
                int.TryParse(table[tableID][subKey], out val);
        }
        return val;
    }

    public float ToF(int tableID, string subKey)
    {
        float val = -1;
        if (table.ContainsKey(tableID))
        {
            if (table[tableID].ContainsKey(subKey))
                float.TryParse(table[tableID][subKey], out val);
        }
        return val;
    }
}
