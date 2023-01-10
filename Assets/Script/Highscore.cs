using System;

[Serializable]
public class Highscore : IComparable<Highscore>
{
    public string Username;
    public string Score;

    public Highscore(string username, string score)
    {
        Username = username;
        Score = score;
    }

    public override int GetHashCode()
    {
        return GetScore().GetHashCode() + GetNome().GetHashCode();
    }

    public override bool Equals(object obj)
    {
        Highscore that = (Highscore)obj;
        return (Score == that.Score) && Username.Equals(that.Username);
    }


    public int CompareTo(Highscore that)
    {
        int diff = Convert.ToInt32(that.Score) - Convert.ToInt32(this.Score);
        if (diff != 0)
            return diff;
        return string.Compare(Username, that.Username, StringComparison.Ordinal);
    }


    public string GetScore()
    {
        return Score;
    }

    public string GetNome()
    {
        return Username;
    }
}