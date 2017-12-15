package edu.uncc.homework4;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.Date;

/**
 * Created by sunand on 11/9/17.
 */

public class SurveyResponse implements Serializable {
    String  UserId;
    String SurveyResponseReceivedTime;
    ArrayList<QuestionResponse> Responses;
    int SurveyId,StudyGroupId;

    public SurveyResponse(int surveyId, String userId, int studyGroupId, String surveyResponseReceivedTime) {
        SurveyId = surveyId;
        UserId = userId;
        StudyGroupId = studyGroupId;
        SurveyResponseReceivedTime = surveyResponseReceivedTime;
        Responses = new ArrayList<>();
    }

    public ArrayList<QuestionResponse> getQuestionResponses() {
        return Responses;
    }

    public void setQuestionResponses(ArrayList<QuestionResponse> questionResponses) {
        Responses = questionResponses;
    }

    public SurveyResponse(){
        Responses = new ArrayList<>();
    }


    public int getSurveyId() {
        return SurveyId;
    }

    public void setSurveyId(int surveyId) {
        SurveyId = surveyId;
    }

    public String getUserId() {
        return UserId;
    }

    public void setUserId(String userId) {
        UserId = userId;
    }

    public int getStudyGroupId() {
        return StudyGroupId;
    }

    public void setStudyGroupId(int studyGroupId) {
        StudyGroupId = studyGroupId;
    }

    public String getSurveyResponseReceivedTime() {
        return SurveyResponseReceivedTime.toString();
    }

    public void setSurveyResponseReceivedTime(String surveyResponseReceivedTime) {
        SurveyResponseReceivedTime = surveyResponseReceivedTime;
    }

    public ArrayList<QuestionResponse> getUserResponseText() {
        return Responses;
    }

    public void setUserResponseText(ArrayList<QuestionResponse> userResponseText) {
        Responses = userResponseText;
    }
}
