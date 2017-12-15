package edu.uncc.homework4;


import java.util.ArrayList;

/**
 * Vinnakota Venkata Ratna Ushaswini
 * Survey
 * 14/12/2017
 */

public class Survey {
    int surveytype, surveyId,studyGroupId;
    String surveyName, response;
    String studyCoordinatorId;

    public int getSurveytype() {
        return surveytype;
    }

    public String getResponse() {
        return response;
    }

    public void setResponse(String response) {
        this.response = response;
    }

    public void setSurveytype(int surveytype) {
        this.surveytype = surveytype;
    }

    public int getSurveyId() {
        return surveyId;
    }

    public void setSurveyId(int surveyId) {
        this.surveyId = surveyId;
    }

    public int getStudyGroupId() {
        return studyGroupId;
    }

    public void setStudyGroupId(int studyGroupId) {
        this.studyGroupId = studyGroupId;
    }

    public String getSurveyName() {
        return surveyName;
    }

    public void setSurveyName(String surveyName) {
        this.surveyName = surveyName;
    }

    public String getStudyCoordinatorId() {
        return studyCoordinatorId;
    }

    public void setStudyCoordinatorId(String studyCoordinatorId) {
        this.studyCoordinatorId = studyCoordinatorId;
    }

    public String getStudyCoordinatorName() {
        return studyCoordinatorName;
    }

    public void setStudyCoordinatorName(String studyCoordinatorName) {
        this.studyCoordinatorName = studyCoordinatorName;
    }

    public ArrayList<SurveyQuestion> getQuestions() {
        return Questions;
    }

    public void setQuestions(ArrayList<SurveyQuestion> questions) {
        Questions = questions;
    }

    String studyCoordinatorName;
    ArrayList<SurveyQuestion> Questions;

}
