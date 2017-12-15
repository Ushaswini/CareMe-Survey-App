package edu.uncc.homework4;

import java.io.Serializable;
import java.util.ArrayList;

/**
 * Created by Nitin on 11/20/2017.
 */

public class SurveyQuestion implements Serializable {


    int questionId;
    String questionText,options;
    double minimum, maximum, stepsize;


    QuestionType questionType;

    public QuestionType getQuestionType() {
        return questionType;
    }

    public void setQuestionType(int questionType) {
        this.questionType = QuestionType.values()[questionType];
    }

    public SurveyQuestion() {
    }

    public int getQuestionId() {
        return questionId;
    }

    public void setQuestionId(int questionId) {
        this.questionId = questionId;
    }



    public String getQuestionText() {
        return questionText;
    }

    public void setQuestionText(String questionText) {
        this.questionText = questionText;
    }

    public String getOptions() {
        return options;
    }

    public void setOptions(String options) {
        this.options = options;
    }

    public double getMinimum() {
        return minimum;
    }

    public void setMinimum(double minimum) {
        this.minimum = minimum;
    }

    public double getMaximum() {
        return maximum;
    }

    public void setMaximum(double maximum) {
        this.maximum = maximum;
    }

    public double getStepsize() {
        return stepsize;
    }

    public void setStepsize(double stepsize) {
        this.stepsize = stepsize;
    }
}

