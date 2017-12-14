package edu.uncc.homework4;

import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.design.widget.NavigationView;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.Toast;

public class NavigationDrawerActivity extends AppCompatActivity
        implements NavigationView.OnNavigationItemSelectedListener {

    private ListView mDrawerList;
    private ArrayAdapter<String> mAdapter;
    SharedPreferences prefs;
    SharedPreferences.Editor prefsEditor;



    private void addDrawerItems() {
        String[] navArray = { "The Coach(Messenger)","My Profile","Logout" };
        mAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_list_item_1, navArray);
        mDrawerList.setAdapter(mAdapter);
    }

    private void ShowDialog(){
        AlertDialog.Builder builder1 = new AlertDialog.Builder(this);
        builder1.setMessage("Allow Care me to send notifications");
        builder1.setCancelable(false);

        builder1.setPositiveButton(
                "Yes",
                new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int id) {

                        prefsEditor.putString(Constants.RECEIVE_NOTIFICATION_BOOLEAN, "Yes");
                        prefsEditor.commit();
                        Intent intent = new Intent(NavigationDrawerActivity.this, RegistrationIntentService.class);
                        startService(intent);
                    }
                });

        builder1.setNegativeButton("No", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                Toast.makeText(NavigationDrawerActivity.this, "Notifications disabled", Toast.LENGTH_SHORT).show();
            }
        });


        AlertDialog alert11 = builder1.create();
        alert11.show();
    }


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_navigation_drawer);

        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        prefs = this.getSharedPreferences(Constants.PREFS,MODE_PRIVATE);
        prefsEditor = prefs.edit();
        String wantsNotification = prefs.getString(Constants.RECEIVE_NOTIFICATION_BOOLEAN,"");

        if (wantsNotification.equals("")) {
            ShowDialog();
        }

        MessagesFragment fragment = new MessagesFragment();
        getSupportFragmentManager().beginTransaction()
                .replace(R.id.content_frame,fragment)
                .commit();



        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(
                this, drawer, toolbar, R.string.navigation_drawer_open, R.string.navigation_drawer_close);
        drawer.setDrawerListener(toggle);
        toggle.syncState();

        NavigationView navigationView = (NavigationView) findViewById(R.id.nav_view);
        navigationView.setNavigationItemSelectedListener(this);
    }

    @Override
    public void onBackPressed() {
        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        if (drawer.isDrawerOpen(GravityCompat.START)) {
            drawer.closeDrawer(GravityCompat.START);
        } else {
            super.onBackPressed();
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.navigation_drawer, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    @SuppressWarnings("StatementWithEmptyBody")
    @Override
    public boolean onNavigationItemSelected(MenuItem item) {
        // Handle navigation view item clicks here.
        int id = item.getItemId();

        if (id == R.id.nav_messenger) {

            MessagesFragment fragment = new MessagesFragment();
            fragment.getMyActivity(NavigationDrawerActivity.this);
            getSupportFragmentManager().beginTransaction()
                    .replace(R.id.content_frame,fragment)
                    .commit();
        } else if (id == R.id.nav_profile) {
            ProfileFragment fragment = new ProfileFragment();
            getSupportFragmentManager().beginTransaction()
                    .replace(R.id.content_frame,fragment)
                    .commit();

        } else if (id == R.id.nav_logout) {

            //TODO:Delete deviceid
            prefsEditor.remove(Constants.AUTH_HEADER);
            prefsEditor.remove(Constants.USERID);
            prefsEditor.remove(Constants.USERNAME);
            prefsEditor.remove(Constants.EMAIL);
            prefsEditor.remove(Constants.REGIONID);
            prefsEditor.remove(Constants.FULLNAME);
            prefsEditor.commit();
            finish();
        }

        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        drawer.closeDrawer(GravityCompat.START);
        return true;
    }

}
