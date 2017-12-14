package edu.uncc.homework4;

import android.content.Context;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.preference.PreferenceManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.TextView;
import android.widget.Toast;

import com.google.gson.Gson;

import org.ocpsoft.prettytime.PrettyTime;

import java.io.IOException;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;
import java.util.TimeZone;

import okhttp3.Call;
import okhttp3.Callback;
import okhttp3.FormBody;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;

import static edu.uncc.homework4.QuestionType.Choice;
import static edu.uncc.homework4.QuestionType.Message;

/**
 * Created by Nitin on 11/14/2017.
 */

public class MessagesRecyclerAdapter extends RecyclerView.Adapter<MessagesRecyclerAdapter.ViewHolder>{


    private OnItemClickListener listener;
    public boolean editMode = true;

    public interface OnItemClickListener {
        void onItemClick(int position,int checkedId, String reply);
    }
    public void setOnItemClickListener(OnItemClickListener listener) {
        this.listener = listener;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {

        public TextView tvMessage;
        public Button BtnSend;
        public RadioButton rbYes;
        public RadioButton rbNo;
        public RadioGroup rgOptions;
        public EditText etReplyMsg;
        public TextView tvTime;
        public ImageView imMessageTick;


        Context vContext;

        public ViewHolder(Context context,final View itemView) {
            super(itemView);

            tvMessage = (TextView) itemView.findViewById(R.id.tvMessage);
            BtnSend = (Button) itemView.findViewById(R.id.btnSendResponse);
            rbYes = (RadioButton) itemView.findViewById(R.id.radioButtonYes);
            rbNo = (RadioButton) itemView.findViewById(R.id.radioButtonNo);
            rgOptions = (RadioGroup) itemView.findViewById(R.id.rgChoice);
            etReplyMsg = (EditText)itemView.findViewById(R.id.etAnswer);
            tvTime = (TextView)itemView.findViewById(R.id.messageTime);
            imMessageTick = (ImageView)itemView.findViewById(R.id.imgResponseTick);

            vContext = context;

            if(BtnSend.isEnabled()){
                if (rgOptions.getCheckedRadioButtonId() != -1 || etReplyMsg.getText() != null){
                    final int position = getAdapterPosition();
                    if (position != RecyclerView.NO_POSITION) {
                        listener.onItemClick(position, rgOptions.getCheckedRadioButtonId(), etReplyMsg.getText().toString());
                    }
                }
            }
        }

    }

    List<SurveyQuestion> messages;
    Context mContext;

    public MessagesRecyclerAdapter(List<SurveyQuestion> messages, Context mContext) {
        this.messages = messages;
        this.mContext = mContext;
    }

    private Context getContext() {
        return mContext;
    }

    @Override
    public MessagesRecyclerAdapter.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {

        Context context = parent.getContext();
        LayoutInflater inflater = LayoutInflater.from(context);
        View contactView = inflater.inflate(R.layout.messages_layout, parent, false);
        ViewHolder viewHolder = new ViewHolder(getContext(),contactView);

        return viewHolder;
    }

    @Override
    public void onBindViewHolder(MessagesRecyclerAdapter.ViewHolder holder, int position) {

        SurveyQuestion surveyQuestion = messages.get(position);
        holder.tvMessage.setText(surveyQuestion.getQuestion());
        SimpleDateFormat simpleDateFormat = new SimpleDateFormat(Constants.DATE_FORMAT);
        try {
            Date messageDate = simpleDateFormat.parse(messages.get(position).getSurveyTime());
            PrettyTime p = new PrettyTime();
            holder.tvTime.setText(p.format(messageDate));
        } catch (ParseException e) {
            e.printStackTrace();
        }

        switch (surveyQuestion.QuestionType)
        {
            case Message:
            {
                holder.etReplyMsg.setVisibility(View.GONE);
                holder.rgOptions.setVisibility(View.GONE);
                holder.tvMessage.setVisibility(View.VISIBLE);
                holder.tvMessage.setText(surveyQuestion.getQuestion());
                holder.imMessageTick.setVisibility(View.GONE);
                holder.BtnSend.setVisibility(View.GONE);
                break;
            }

            case Choice:
            {
                holder.tvMessage.setVisibility(View.VISIBLE);
                holder.rgOptions.setVisibility(View.VISIBLE);
                holder.BtnSend.setVisibility(View.VISIBLE);
                holder.etReplyMsg.setVisibility(View.GONE);
                holder.rgOptions.clearCheck();

                if (surveyQuestion.getResponse() != null)
                {
                    holder.tvMessage.setText(surveyQuestion.getQuestion());
                    holder.BtnSend.setEnabled(false);
                    holder.imMessageTick.setVisibility(View.VISIBLE);
                    holder.rgOptions.clearCheck();
                    if (surveyQuestion.getResponse() == ("Yes"))
                    {
                        holder.rbYes.setChecked(true);
                    }
                    else
                    {
                        holder.rbNo.setChecked(true);
                    }
                    holder.rgOptions.setEnabled(false);
                    holder.rbNo.setEnabled(false);
                    holder.rbYes.setEnabled(false);
                }
                else
                {
                    holder.tvMessage.setText(surveyQuestion.getQuestion());
                    holder.BtnSend.setEnabled(true);
                    holder.rgOptions.setEnabled(true);
                    holder.imMessageTick.setVisibility(View.GONE);
                }
                break;
            }

            case TextEntry:
            {
                holder.tvMessage.setVisibility(View.VISIBLE);
                holder.rgOptions.setVisibility(View.GONE);
                holder.BtnSend.setVisibility(View.VISIBLE);
                holder.etReplyMsg.setVisibility(View.VISIBLE);

                if (surveyQuestion.getResponse() != null)
                {
                    holder.tvMessage.setText(surveyQuestion.getQuestion());
                    holder.etReplyMsg.setEnabled(false);
                    holder.etReplyMsg.setText(surveyQuestion.getResponse());
                    holder.BtnSend.setEnabled(false);
                    holder.imMessageTick.setVisibility(View.VISIBLE);
                }
                else
                {
                    holder.tvMessage.setText(surveyQuestion.getQuestion());
                    holder.etReplyMsg.setEnabled(true);
                    holder.BtnSend.setEnabled(true);
                    holder.imMessageTick.setVisibility(View.GONE);
                }
                break;
            }
        }

    }

    @Override
    public int getItemCount() {
        return messages.size();
    }


}

