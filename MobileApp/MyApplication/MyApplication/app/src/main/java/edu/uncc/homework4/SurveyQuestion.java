package edu.uncc.homework4;

import java.io.Serializable;

/**
 * Created by Nitin on 11/20/2017.
 */

public class SurveyQuestion implements Serializable {

    String question, userId, surveyTime;
    String response,studyGrpId,surveyId,responseDate;
    QuestionType QuestionType;

    public SurveyQuestion() {
    }

    public String getQuestion() {
        return question;
    }

    public void setQuestion(String question) {
        this.question = question;
    }

    public String getResponse() {
        return response;
    }

    public void setResponse(String response) {
        this.response = response;
    }

    public String getStudyGrpId() {
        return studyGrpId;
    }

    public void setStudyGrpId(String studyGrpId) {
        this.studyGrpId = studyGrpId;
    }

    public String getSurveyId() {
        return surveyId;
    }

    public void setSurveyId(String surveyId) {
        this.surveyId = surveyId;
    }

    public String getResponseDate() {
        return responseDate;
    }

    public void setResponseDate(String responseDate) {
        this.responseDate = responseDate;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public QuestionType getQuesType() {
        return QuestionType;
    }

    public void setQuesType(int quesType) {

        this.QuestionType = QuestionType.values()[quesType];
    }

    public String getSurveyTime() {
        return surveyTime;
    }

    public void setSurveyTime(String surveyTime) {
        this.surveyTime = surveyTime;
    }
}

