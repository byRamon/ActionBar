using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content;
using Android.Views;

namespace ActionBar
{
    //[Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.NoActionBar", MainLauncher = true)]
    [Register("com.casa.MainActivity")]
    public class MainActivity : AppCompatActivity
    {        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            int apiLevel = (int)Build.VERSION.SdkInt;
            string apiLevelName = Build.VERSION.SdkInt.ToString();
            /*if (Build.VERSION.SdkInt >= BuildVersionCodes.IceCreamSandwich)
                SupportActionBar.Title = string.Format( "Action Bar en ApiLevel {0}({1})",apiLevel,apiLevelName);
            else
                SupportActionBar.Title = string.Format("Action Bar en ApiLevel {0}({1}) mostrado gracias a SupportActionBar", apiLevel,apiLevelName);            
                */
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.my_toolbar);
            toolbar.Title = string.Format("Action Bar en ApiLevel {0}({1})", apiLevel, apiLevelName);
            SetSupportActionBar(toolbar);
            Button my_button = FindViewById<Button>(Resource.Id.my_button);
            my_button.Click += delegate { StartActivity(typeof(ActividadHija)); };

            Button my_button2 = FindViewById<Button>(Resource.Id.my_button2);
            my_button2.Click += delegate { StartActivity(typeof(ActividadHija2)); };

            Button my_buttonContextual = FindViewById<Button>(Resource.Id.my_buttonContextual);
            my_buttonContextual.Click += (s, e) => Toast.MakeText(this, "Click", ToastLength.Short).Show();
            this.RegisterForContextMenu(my_buttonContextual);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);
            MenuInflater.Inflate(Resource.Menu.opciones_contextuales, menu);
            menu.SetHeaderIcon(Resource.Drawable.agregar);
            menu.SetHeaderTitle("Menu contextual");

        }
        public override bool OnContextItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.agregar)
                Toast.MakeText(this, "Agregado", ToastLength.Long).Show();
            return base.OnContextItemSelected(item);
        }
    }

    [Activity(Label = "Actividad Hija")]
    [MetaData("android.support.PARENT_ACTIVITY", Value = "com.casa.MainActivity")]
    [Register("com.casa.ActividadHija")]
    public class ActividadHija : AppCompatActivity, Android.Support.V7.View.ActionMode.ICallback
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.actividad_hija);
            SupportActionBar.Title = "Actividad hija";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            Android.Support.V7.View.ActionMode actionMode = null;

            Button button1 = FindViewById<Button>(Resource.Id.button1);
            button1.Click += delegate { StartActivity(typeof(ActividadHija2)); };
            button1.LongClick += (o, e) => {
                actionMode = StartSupportActionMode(this);
            };

            Button button2 = FindViewById<Button>(Resource.Id.button2);
            button2.LongClick += (o, e) => {
                actionMode = StartSupportActionMode(this);
            };
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.opciones,menu);
            IMenuItem searchItem = menu.FindItem(Resource.Id.search);
            var searchView1 = searchItem.ActionView as Android.Support.V7.Widget.SearchView;
            searchView1.QueryHint = "Buscando";
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if(item.ItemId == Resource.Id.acercade)
            {
                Toast.MakeText(this,"Acerca de",ToastLength.Short).Show();
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public bool OnActionItemClicked(Android.Support.V7.View.ActionMode mode, IMenuItem item)
        {
            if (item.ItemId == Resource.Id.acercade)
            {
                Toast.MakeText(this, "Item Acerca de", ToastLength.Short).Show();
                return true;
            }
            return false;
        }

        public bool OnCreateActionMode(Android.Support.V7.View.ActionMode mode, IMenu menu)
        {
            mode.MenuInflater.Inflate(Resource.Menu.opciones, menu);
            return true;
        }

        public void OnDestroyActionMode(Android.Support.V7.View.ActionMode mode)
        {
        }

        public bool OnPrepareActionMode(Android.Support.V7.View.ActionMode mode, IMenu menu)
        {
            return false;
        }

        public override Intent SupportParentActivityIntent => base.SupportParentActivityIntent;
    }

    [Activity(Label = "Actividad Hija 2")]
    [MetaData("android.support.PARENT_ACTIVITY", Value = "com.casa.ActividadHija")]
    public class ActividadHija2 : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.actividad_hija2);
            SupportActionBar.Title = "Actividad hija 2";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override Intent SupportParentActivityIntent => base.SupportParentActivityIntent;

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.opciones,menu);            
            return base.OnCreateOptionsMenu(menu);
        }
    }
}