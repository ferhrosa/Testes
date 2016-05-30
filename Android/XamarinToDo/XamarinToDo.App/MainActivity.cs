using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace XamarinToDo.App
{
    [Activity(Label = "Tarefas", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        Button button;
        ListView listView1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            this.button = FindViewById<Button>(Resource.Id.MyButton);

            this.button.Click += buttonClick;

            this.listView1 = FindViewById<ListView>(Resource.Id.listView1);
        }

        private void buttonClick(object sender, EventArgs e)
        {
            this.button.Text = string.Format("{0} clicks!", count++);

            //this.gridView
            //this.listView1.Adapter
        }
    }
}

