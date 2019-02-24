using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace Manybits
{
    public enum LevelAccess {LA_COMPLITED, LA_LOCKED, LA_UNLOCKED}

    public class Level
    {
        public List<Vector2> points = new List<Vector2>();
        public List<DataLine> lines = new List<DataLine>();
        public int moves = 0;
        public float time = 0.0f;
        public int stars = 0;
        public int number;
        public LevelAccess access = LevelAccess.LA_LOCKED;

        public void Load(XmlNode node)
        {
            XmlNodeList pointsList = node.SelectNodes("point");
            foreach (XmlNode item in pointsList)
            {
                float tmpX = float.Parse(item.Attributes["x"].Value.Replace(",", "."));
                float tmpY = float.Parse(item.Attributes["y"].Value.Replace(",", "."));
                Vector2 point = new Vector2(tmpX, tmpY);

                points.Add(point);
            }

            XmlNodeList linesList = node.SelectNodes("line");
            foreach (XmlNode item in linesList)
            {
                DataLine line = new DataLine();
                line.point1Index = int.Parse(item.Attributes["point1"].Value);
                line.point2Index = int.Parse(item.Attributes["point2"].Value);
                lines.Add(line);
            }
        }
    }
}
