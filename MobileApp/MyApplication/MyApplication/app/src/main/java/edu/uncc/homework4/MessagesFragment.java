package edu.uncc.homework4;

import android.app.Activity;
import android.content.Context;
import android.content.SharedPreferences;
import android.net.Uri;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.google.gson.Gson;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.TimeZone;

import okhttp3.Call;
import okhttp3.Callback;
import okhttp3.FormBody;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;

import static edu.uncc.homework4.QuestionType.*;


/**
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link MessagesFragment.OnFragmentInteractionListener} interface
 * to handle interaction events.
 */
public class MessagesFragment extends Fragment implements MessagesRecyclerAdapter.OnItemClickListener {

    MessagesRecyclerAdapter messagesRecyclerAdapter;
    ArrayList<String> messagesList;
    ArrayList<SurveyQuestion> surveyQuestionArrayList;
    RecyclerView messagesView;
    Activity myActivity;
    SharedPreferences prefs;
    SharedPreferences.Editor prefsEditor;
    String UserId = "";
    String Access_Token = "";


    private OnFragmentInteractionListener mListener;

    public MessagesFragment() {
        // Required empty public constructor
    }

    public void getMyActivity(Activity activity) {
        this.myActivity = activity;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment

        return inflater.inflate(R.layout.fragment_messages, container, false);

    }

    // TODO: Rename method, update argument and hook method into UI event
    public void onButtonPressed(Uri uri) {
        if (mListener != null) {
            mListener.onFragmentInteraction(uri);
        }
    }

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
    }


    private void GetSurveysAsync() {

        OkHttpClient client = new OkHttpClient();
        Request request = new Request.Builder()
                .url(Constants.GET_SURVEYS_URL + UserId)
                .header("Authorization", "Bearer " + Access_Token)
                .build();


        Log.d("demo", "final" + UserId);
        client.newCall(request).enqueue(new Callback() {
            @Override
            public void onFailure(Call call, IOException e) {
                Log.d("demo", "failure");
            }

            @Override
            public void onResponse(Call call, Response response) throws IOException {

                Log.d("demo", "success");
                messagesList = new ArrayList<>();
                final ArrayList<SurveyQuestion> surveysList = new ArrayList<SurveyQuestion>();
                final String myResponse = response.body().string();
                Log.d("demo", myResponse + " hello " + messagesList);

                try {
                    JSONObject jsonObject = new JSONObject(myResponse);
                    JSONArray jsonArray = jsonObject.getJSONArray("SurveysResponded");
                    for (int i = 0; i < jsonArray.length(); i++) {
                        JSONObject jsonO = jsonArray.getJSONObject(i);
                        SurveyQuestion sq = new SurveyQuestion();
                        sq.setQuestion(jsonO.getString("QuestionText"));
                        sq.setResponse(jsonO.getString("ResponseText"));
                        sq.setSurveyId(jsonO.getString("SurveyId"));
                        sq.setUserId(UserId);
                        sq.setQuesType(jsonO.getInt("QuestionType"));
                        sq.setSurveyTime(jsonO.getString("ResponseReceivedTime"));
                        surveysList.add(sq);
                    }
                    JSONArray jsonArray1 = jsonObject.getJSONArray("Surveys");
                    for (int i = 0; i < jsonArray1.length(); i++) {
                        JSONObject jsonO = jsonArray1.getJSONObject(i);
                        SurveyQuestion sq = new SurveyQuestion();
                        sq.setQuestion(jsonO.getString("QuestionText"));
                        sq.setSurveyId(jsonO.getString("SurveyId"));
                        sq.setStudyGrpId(jsonO.getString("StudyGroupId"));
                        sq.setUserId(UserId);
                        sq.setQuesType(jsonO.getInt("QuestionType"));
                        sq.setSurveyTime(jsonO.getString("SurveyCreatedTime"));
                        sq.setResponse("");
                        surveysList.add(sq);
                    }
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                getActivity().runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        Log.d("demo", "success adapter");

                        surveyQuestionArrayList.clear();
                        surveyQuestionArrayList.addAll(surveysList);
                        messagesRecyclerAdapter.notifyDataSetChanged();
                    }
                });


            }
        });
    }

    @Override
    public void onActivityCreated(@Nullable Bundle savedInstanceState) {
        super.onActivityCreated(savedInstanceState);
        prefs = getActivity().getSharedPreferences(Constants.PREFS, Context.MODE_PRIVATE);

        Access_Token = prefs.getString(Constants.AUTH_HEADER, "");
        UserId = prefs.getString(Constants.USERID, "");
        surveyQuestionArrayList = new ArrayList<>();
        messagesRecyclerAdapter = new MessagesRecyclerAdapter(surveyQuestionArrayList, getContext());
        messagesRecyclerAdapter.setOnItemClickListener(MessagesFragment.this);
        messagesView = (RecyclerView) getActivity().findViewById(R.id.recyclerViewResult);

        messagesView.setAdapter(messagesRecyclerAdapter);
        messagesView.setLayoutManager(new LinearLayoutManager(getContext()));
        GetSurveysAsync();
    }

    @Override
    public void onDetach() {
        super.onDetach();
        mListener = null;
    }

    @Override
    public void onItemClick(int position,int checkedId, String reply) {
//TODO:Send response and reload data

        SharedPreferences sharedPref = getActivity().getSharedPreferences(Constants.PREFS,Context.MODE_PRIVATE);
        String access_token = sharedPref.getString(Constants.AUTH_HEADER,"");
        SurveyQuestion data = surveyQuestionArrayList.get(position);
        String replyText = "";
        switch (data.QuestionType)
        {
            case Choice:
            {
                if (checkedId == R.id.radioButtonYes) replyText = "Yes";
                else replyText = "No";

                break;
            }
            case TextEntry:
            {
                replyText = reply;
                break;
            }
        }
        RequestBody formBody = new FormBody.Builder()
                .add("UserId", data.getUserId())
                .add("StudyGroupId", data.getStudyGrpId())
                .add("SurveyId", data.getSurveyId())
                .add("UserResponseText",replyText )
                .add("SurveyResponseReceivedTime", (new Date()).toString())
                .build();

        Request request = new Request.Builder()
                .url(Constants.POST_RESPONSE_URL)
                .header("Content-Type","application/x-www-form-urlencoded")
                .header("Authorization", "Bearer "+access_token)
                .post(formBody)
                .build();


        OkHttpClient client = new OkHttpClient();
        client.newCall(request).enqueue(new Callback() {
            @Override
            public void onFailure(Call call, IOException e) {
                Log.d("demo","response failure");
                // Toast.makeText(getContext(),"Error in sending response",Toast.LENGTH_SHORT);
            }

            @Override
            public void onResponse(Call call, Response response) throws IOException {
                // Toast.makeText(getContext(),"Response sent successfully !!",Toast.LENGTH_SHORT);
                Log.d("demo","response success");
                GetSurveysAsync();
            }
        });



    }

    /**
     * This interface must be implemented by activities that contain this
     * fragment to allow an interaction in this fragment to be communicated
     * to the activity and potentially other fragments contained in that
     * activity.
     * <p>
     * See the Android Training lesson <a href=
     * "http://developer.android.com/training/basics/fragments/communicating.html"
     * >Communicating with Other Fragments</a> for more information.
     */
    public interface OnFragmentInteractionListener {
        // TODO: Update argument type and name
        void onFragmentInteraction(Uri uri);
    }
}
