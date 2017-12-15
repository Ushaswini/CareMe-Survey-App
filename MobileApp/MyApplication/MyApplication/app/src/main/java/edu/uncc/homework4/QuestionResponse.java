package edu.uncc.homework4;


/**
 * Vinnakota Venkata Ratna Ushaswini
 * QuestionResponse
 * 15/12/2017
 */

public class QuestionResponse {
    String  responseText;
    int questionId;

    public QuestionResponse(int questionId, String responseText) {
        this.questionId = questionId;
        this.responseText = responseText;
    }

    public int getQuestionId() {
        return questionId;
    }

    public void setQuestionId(int questionId) {
        this.questionId = questionId;
    }

    public String getResponseText() {
        return responseText;
    }

    public void setResponseText(String responseText) {
        this.responseText = responseText;
    }
}
