using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLayout : MonoBehaviour
{
    public GameObject stage_go;
    public GameObject round_go;
    public GameObject question_go;
    public Image first_number;
    public Image first_image;
    public Image operator_image;
    public Image second_number;
    public Image second_image;
    public Image answer_image;
    public GameObject left_option;
    public Image left_image;
    public GameObject right_option;
    public Image right_image;
    public GameObject correct_go;
    public GameObject error_go;
    //public Text question_number_text;

    string game_image_path;
    string question_mark_path;

    // Start is called before the first frame update
    void Start()
    {
        game_image_path = "GameImage/{0}";
        question_mark_path = string.Format(game_image_path, "QuestionMark");
    }

    public void showStage(bool _is_show)
    {
        stage_go.SetActive(_is_show);
    }

    public void showRound(int _round, bool _is_show)
    {
        round_go.SetActive(_is_show);
        string _round_path = string.Format(game_image_path, string.Format("Round{0}", _round + 1));
        round_go.GetComponent<Image>().sprite = Resources.Load<Sprite>(_round_path);
    }

    public void correctFeedback(bool _is_show)
    {
        correct_go.SetActive(_is_show);
    }

    public void errorFeedback(bool _is_show)
    {
        error_go.SetActive(_is_show);
    }

    //public void setQuestionInfo(int _curr, int _total)
    //{
    //    //question_number_text.text = string.Format("題數： {0} / {1}", _curr, _total);
    //}

    #region Question and option
    public void showQuestion(bool _is_show)
    {
        question_go.SetActive(_is_show);
    }

    public void setFirstNumber(string _path, Box _box)
    {
        first_number.sprite = Resources.Load<Sprite>(string.Format(game_image_path, _box));
        first_image.sprite = Resources.Load<Sprite>(_path);
    }

    public void setOperator(string _operator)
    {
        if (_operator.Equals("+"))
        {
            operator_image.sprite = Resources.Load<Sprite>(string.Format(game_image_path, "Plus"));
        }
        else if (_operator.Equals("-"))
        {
            operator_image.sprite = Resources.Load<Sprite>(string.Format(game_image_path, "Minus"));
        }
        else
        {
            print("Error operator type.");
        }
    }

    public void setSecondNumber(string _path, Box _box)
    {
        second_number.sprite = Resources.Load<Sprite>(string.Format(game_image_path, _box));
        second_image.sprite = Resources.Load<Sprite>(_path);
    }

    public void setTarget(string _path)
    {
        answer_image.sprite = Resources.Load<Sprite>(_path);
    }

    public void setLeftOption(string _path)
    {
        showLeftOption(true);
        left_image.sprite = Resources.Load<Sprite>(_path);
    }

    public void showLeftOption(bool _is_show)
    {
        left_option.SetActive(_is_show);
    }

    public void setRightOption(string _path)
    {
        showRightOption(true);
        right_image.sprite = Resources.Load<Sprite>(_path);
    }

    public void showRightOption(bool _is_show)
    {
        right_option.SetActive(_is_show);
    }

    public void setGame3(int _round)
    {
        string game3 = "Game3/{0}{1}";
        setFirstNumber(string.Format(game3, "First", _round + 1), Box.Black);
        setOperator("-");
        setSecondNumber(question_mark_path, Box.Red);
        setTarget(string.Format(game3, "Target", _round + 1));
        setLeftOption(string.Format(game3, "Left", _round + 1));
        setRightOption(string.Format(game3, "Right", _round + 1));
    }

    public void setGame4(int _round)
    {
        string game4 = "Game4/{0}{1}";
        setFirstNumber(string.Format(game4, "First", _round + 1), Box.Black);
        setOperator("-");
        setSecondNumber(question_mark_path, Box.Red);
        setTarget(string.Format(game4, "Target", _round + 1));
        setLeftOption(string.Format(game4, "Left", _round + 1));
        setRightOption(string.Format(game4, "Right", _round + 1));
    }

    public void setGame5(int _round) 
    {
        string game5 = "Game5/{0}{1}";
        setFirstNumber(string.Format(game5, "First", _round + 1), Box.Black);
        setOperator("+");
        setSecondNumber(question_mark_path, Box.Red);
        setTarget(string.Format(game5, "Target", _round + 1));
        setLeftOption(string.Format(game5, "Left", _round + 1));
        setRightOption(string.Format(game5, "Right", _round + 1));
    }

    public void setGame6(int _round)
    {
        string game6 = "Game6/{0}{1}";
        setFirstNumber(string.Format(game6, "First", _round + 1), Box.Black);
        setOperator("-");
        setSecondNumber(question_mark_path, Box.Red);
        setTarget(string.Format(game6, "Target", _round + 1));
        setLeftOption(string.Format(game6, "Left", _round + 1));
        setRightOption(string.Format(game6, "Right", _round + 1));
    }
    #endregion
}
