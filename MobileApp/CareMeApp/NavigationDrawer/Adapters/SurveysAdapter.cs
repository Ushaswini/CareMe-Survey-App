using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
//using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using NavigationDrawer;
using NavigationDrawer.Models;
using System.Net.Http;
using Android.Icu.Text;
using Android.Preferences;

namespace edu.uncc.homework4
{
    public class SurveysAdapter : RecyclerView.Adapter
    {
        List<Message> mData;
        Context mContext;
        OnSendClickListener mListener;       
        ISharedPreferences prefs;

        //Associated Objects
        public interface OnSendClickListener
        {
            void OnResponseSendClicked(int position, int checkedId, string reply);
        }

        public class ViewHolder : RecyclerView.ViewHolder
        {
            public readonly TextView tvMessage;
            public readonly EditText etReplyMsg;
            public readonly RadioGroup rgOptions;
            public readonly Button BtnSend;
            public readonly TextView tvTime;
            public readonly ImageView imMessageTick;
            public readonly RadioButton rbYes;
            public readonly RadioButton rbNo;

            public ViewHolder(View itemView, Action<int,int,string> clickListerner) : base(itemView)
            {
                tvMessage = itemView.FindViewById<TextView>(Resource.Id.tvMessage);
                etReplyMsg = itemView.FindViewById<EditText>(Resource.Id.etAnswer);
                rgOptions = itemView.FindViewById<RadioGroup>(Resource.Id.rgChoice);
                BtnSend = itemView.FindViewById<Button>(Resource.Id.btnSendResponse);
                tvTime = ItemView.FindViewById<TextView>(Resource.Id.messageTime);
                imMessageTick = ItemView.FindViewById<ImageView>(Resource.Id.imgResponseTick);
                rbYes = ItemView.FindViewById<RadioButton>(Resource.Id.radioButtonYes);
                rbNo = ItemView.FindViewById<RadioButton>(Resource.Id.radioButtonNo);

                BtnSend.Click += (sender, e) =>
                {
                    if (((View)sender).Enabled)
                    {
                        if (rgOptions.CheckedRadioButtonId != -1 || etReplyMsg.Text != null)
                            clickListerner(AdapterPosition, rgOptions.CheckedRadioButtonId, etReplyMsg.Text);                        
                    }
                };

            }


        }

        void OnResponseSendClicked(int position, int checkedId, string reply)
        {
            mListener.OnResponseSendClicked(position, checkedId, reply);
        }

        public SurveysAdapter(List<Message> messages, Context mContext, OnSendClickListener mListener)
        {
            mData = messages;
            this.mContext = mContext;
            this.mListener = mListener;
            prefs = PreferenceManager.GetDefaultSharedPreferences(mContext);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var vi = LayoutInflater.From(parent.Context);
            var v = vi.Inflate(Resource.Layout.dashboard_row_item, parent, false);
            return new ViewHolder(v, OnResponseSendClicked);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holderRaw, int position)
        {
            var holder = (ViewHolder)holderRaw;
            var message = mData[position];
            holder.tvTime.Text = PrettyTimeFormat.GetPrettyDate(message.TimeToDisplay,CONSTANTS.DATE_FORMAT);

            switch (message.QuestionType)
            {
                case QuestionType.Message:
                    {
                        holder.etReplyMsg.Visibility = ViewStates.Gone;
                        holder.rgOptions.Visibility = ViewStates.Gone;
                        holder.tvMessage.Visibility = ViewStates.Visible;
                        holder.tvMessage.Text = message.QuestionText;                        
                        holder.imMessageTick.Visibility = ViewStates.Gone;
                        holder.BtnSend.Visibility = ViewStates.Gone;
                        break;
                    }

                case QuestionType.Choice:
                    {
                        holder.tvMessage.Visibility = ViewStates.Visible;
                        holder.rgOptions.Visibility = ViewStates.Visible;
                        holder.BtnSend.Visibility = ViewStates.Visible;
                        holder.etReplyMsg.Visibility = ViewStates.Gone;
                        holder.rgOptions.ClearCheck();

                        if (message.ResponseId != null)
                        {
                            holder.tvMessage.Text = message.QuestionText;
                            holder.BtnSend.Enabled = false;                            
                            holder.imMessageTick.Visibility = ViewStates.Visible;
                            holder.rgOptions.ClearCheck();
                            if (message.ResponseText.Equals("Yes"))
                            {
                                holder.rbYes.Checked = true;
                            }
                            else
                            {
                                holder.rbNo.Checked = true;
                            }
                            holder.rgOptions.Enabled = false;
                            holder.rbNo.Enabled = false;
                            holder.rbYes.Enabled = false;
                        }
                        else
                        {
                            holder.tvMessage.Text = message.QuestionText;
                            holder.BtnSend.Enabled = true;
                            holder.rgOptions.Enabled = true;
                            holder.imMessageTick.Visibility = ViewStates.Gone;
                        }
                        break;
                    }

                case QuestionType.TextEntry:
                    {
                        holder.tvMessage.Visibility = ViewStates.Visible;
                        holder.rgOptions.Visibility = ViewStates.Gone;
                        holder.BtnSend.Visibility = ViewStates.Visible;
                        holder.etReplyMsg.Visibility = ViewStates.Visible;

                        if (message.ResponseId != null)
                        {
                            holder.tvMessage.Text = message.QuestionText;
                            holder.etReplyMsg.Enabled = false;
                            holder.etReplyMsg.Text = message.ResponseText;
                            holder.BtnSend.Enabled = false;
                            holder.imMessageTick.Visibility = ViewStates.Visible;
                        }
                        else
                        {
                            holder.tvMessage.Text = message.QuestionText;
                            holder.etReplyMsg.Enabled = true;
                            holder.BtnSend.Enabled = true;
                            holder.imMessageTick.Visibility = ViewStates.Gone;
                        }
                        break;
                    }

            }
        }

        public override int ItemCount
        {
            get
            {
                return mData.Count;
            }
        }


        public void UpdateData(List<Message> messages)
        {
            messages.OrderBy(m => m.TimeToDisplay).ThenBy(m => DateTime.Parse(m.TimeToDisplay));
            this.mData.Clear();
            this.mData.AddRange(messages);
            this.NotifyDataSetChanged();
        }
    }
}