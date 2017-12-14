package edu.uncc.homework4;

import android.app.IntentService;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.preference.PreferenceManager;
import android.support.annotation.Nullable;
import android.util.Log;

import com.google.android.gms.gcm.GoogleCloudMessaging;
import com.google.android.gms.iid.InstanceID;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;

import okhttp3.Call;
import okhttp3.Callback;
import okhttp3.FormBody;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;

/**
 * Created by Nitin on 11/19/2017.
 */

public class RegistrationIntentService extends IntentService {
    SharedPreferences prefs;

    String access_token, user_id;
    public RegistrationIntentService(){
        super("");
    }

    /**
     * Creates an IntentService.  Invoked by your subclass's constructor.
     *
     * @param name Used to name the worker thread, important only for debugging.
     */
    public RegistrationIntentService(String name) {
        super(name);
    }

    @Override
    protected void onHandleIntent(@Nullable Intent intent) {

        prefs = getApplicationContext().getSharedPreferences(Constants.PREFS, Context.MODE_PRIVATE);
        final SharedPreferences.Editor editor = prefs.edit();

        access_token = prefs.getString(Constants.AUTH_HEADER,"");
        user_id = prefs.getString(Constants.USERID,"");

        InstanceID instanceID = InstanceID.getInstance(this);
        String token = null;

        try {
            token = instanceID.getToken(getApplicationContext().getString(R.string.gcm_defaultSenderId),
                    GoogleCloudMessaging.INSTANCE_ID_SCOPE, null);
            editor.putString(Constants.RECEIVE_NOTIFICATION_BOOLEAN,"Yes");
            editor.commit();
        } catch (IOException e) {
            e.printStackTrace();
        }

        sendDeviceID(token, user_id);
    }

    public void sendDeviceID(String device_id, String user_id){

        RequestBody formBody = new FormBody.Builder()
                .add("UserId", user_id)
                .add("DeviceId", device_id)
                .build();

        Request request = new Request.Builder()
                .url(Constants.POST_DEVICEID_URL)
                .header("Authorization", "Bearer "+ access_token)
                .post(formBody)
                .build();

        OkHttpClient client = new OkHttpClient();

        client.newCall(request).enqueue(new Callback() {
            @Override
            public void onFailure(Call call, IOException e) {

            }

            @Override
            public void onResponse(Call call, Response response) throws IOException {
                Log.d("demo","refresh token " + response.body().string() );
            }
        });
    }
}
