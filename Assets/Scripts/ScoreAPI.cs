using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.IO;
using System.Text;

public class ScoreAPI : MonoBehaviour
{
    public static Scoreboard getScoreboard(){
      try {
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://bloomenetwork.fr:5000/scoreboard");
        HttpWebResponse response = (HttpWebResponse) request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log(jsonResponse);
        Scoreboard scoreboard = JsonUtility.FromJson<Scoreboard>(jsonResponse);
        return scoreboard;
      }catch (Exception e) {
        return null;
      }

    }

    public static bool sendScore(String name, int score) {
      HttpWebRequest request = (HttpWebRequest) WebRequest.Create("http://bloomenetwork.fr:5000/scoreboard");

      var postData = "name=" + Uri.EscapeDataString(name) + "&score=" + Uri.EscapeDataString(score.ToString());
      var data = Encoding.ASCII.GetBytes(postData);

      request.Method = "POST";
      request.ContentType = "application/x-www-form-urlencoded";
      request.ContentLength = data.Length;
      try {
        using (var stream = request.GetRequestStream()) {
            stream.Write(data, 0, data.Length);
        }
        var response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        Debug.Log(reader.ReadToEnd());
      } catch(Exception e) {
        Debug.Log(e);
        return false;
      }
      return true;
    }
}

[System.Serializable]
public class Score {
  public int id;
  public string name;
  public int score;
  public DateTime date;
  public int alignement;
}
[System.Serializable]
public class Scoreboard {
  public Score[] scores;

  public Score getScore(int i){
    return scores[i];
  }

  public int Length(){
    return scores.Length;
  }
}
