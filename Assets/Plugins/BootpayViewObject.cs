using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#if UNITY_2018_4_OR_NEWER
using UnityEngine.Networking;
#endif


using Callback = System.Action<string>;

public class BootpayViewObject : MonoBehaviour
{
    //Callback callback;
    Callback onConfirm;
    Callback onDone;
    Callback onCancel;
    Callback onReady;
    Callback onClose;


    Callback callbackRestToken;
    Callback callbackEasyPayUserToken;

#if UNITY_IPHONE
    IntPtr webView;


    [DllImport("__Internal")]
    private static extern IntPtr _CBootpayWebviewPlugin_Request(string gameObject, string payload, string user, string items, string extra);


    [DllImport("__Internal")]
    private static extern void _CBootpayWebviewPlugin_TransactionConfirm(IntPtr instance, string data);


    [DllImport("__Internal")]
    private static extern void _CBootpayWebviewPlugin_RemovePaymentWindow(IntPtr instance);

    [DllImport("__Internal")]
    private static extern void _CBootpayWebviewPlugin_Dismiss(IntPtr instance);


#elif UNITY_ANDROID
    AndroidJavaObject webView;
#endif



    // Use this for initialization
    //void Start() { }


    //// Update is called once per frame
    void Update()
    {
        if (webView == null)
            return; 
    }

    public void Init(Callback confirm, Callback done, Callback cancel, Callback ready, Callback close)
    {
        onConfirm = confirm;
        onDone = done;
        onCancel = cancel;
        onReady = ready;
        onClose = close; 
    }

   

    public void Request(string payload, string user, string items, string extra) {

        //Debug.Log(string.Format("Bootpay Webview Start name {0}", name));

#if UNITY_IPHONE
        webView = _CBootpayWebviewPlugin_Request(name, payload, user, items, extra);

#elif UNITY_ANDROID
        if (onCancel != null) onCancel("cancel test");

        webView = new AndroidJavaObject("kr.co.bootpay.unity.CBootpayWebviewPlugin");
        webView.Call("Request", name, payload, user, items, extra);
#endif
    }

    public void TransactionConfirm(string data) {
#if UNITY_IPHONE
        if (webView == IntPtr.Zero) return;
        _CBootpayWebviewPlugin_TransactionConfirm(webView, data);
#elif UNITY_ANDROID
        if (webView == null) return;
        webView.Call("TransactionConfirm", data);
#endif
    }
 

    public void RemovePaymentWindow() {
#if UNITY_IPHONE
        if (webView == IntPtr.Zero) return;
        _CBootpayWebviewPlugin_RemovePaymentWindow(webView);
#elif UNITY_ANDROID
        if (webView == null) return;
        webView.Call("RemovePaymentWindo");
#endif
    }

    public void Dismiss() {
#if UNITY_IPHONE
        if (webView == IntPtr.Zero) return;
        _CBootpayWebviewPlugin_Dismiss(webView);
#elif UNITY_ANDROID
        if (webView == null) return;
        webView.Call("Dismiss");
#endif
    }

    //callback from native call
    public void CallOnError(string data)
    {
        Debug.Log(string.Format("Bootpay Webview CallOnError{0}", data));
        if (onConfirm != null) onConfirm(data);    
    }

    public void CallOnReady(string data)
    {
        Debug.Log(string.Format("Bootpay Webview CallOnReady{0}", data));
        if (onReady != null) onReady(data);
    }

    public void CallOnClose(string data)
    {
        Debug.Log(string.Format("Bootpay Webview CallOnClose{0}", data));
        if (onClose != null) onClose(data);
    }

    public void CallOnConfirm(string data)
    {
        Debug.Log(string.Format("Bootpay Webview CallOnConfirm{0}", data));
        if (onConfirm != null) onConfirm(data);
    }

    public void CallOnCancel(string data)
    {
        Debug.Log(string.Format("Bootpay Webview CallOnCancel{0}", data));
        if (onCancel != null) onCancel(data);
    }
 

    public void CallOnDone(string data)
    {
        Debug.Log(string.Format("Bootpay Webview CallOnDone{0}", data));
        if (onDone != null) onDone(data);
    }


    //set callback from rest api
    public void SetRestCallBack(Callback restToken, Callback userToken)
    {
        callbackRestToken = restToken;
        callbackEasyPayUserToken = userToken;
    }

    //callback from rest api
    public void CallRestToken(string data)
    {
        Debug.Log(string.Format("Bootpay callbackRestToken{0}", data));
        if (callbackRestToken != null) callbackRestToken(data);
    }

    //callback from rest api
    public void CallEasyPayUserToken(string data)
    {
        Debug.Log(string.Format("Bootpay callbackEasyPayUserToken{0}", data));
        if (callbackEasyPayUserToken != null) callbackEasyPayUserToken(data);
    } 
}

public class BootpayUser
{
    public string user_id;
    public string username;
    public string email;
    public int gender;
    public string birth;
    public string phone;
    public string area;

    public BootpayUser() { }
}

public class BootpayItem
{
    public string item_name;
    public int qty;
    public string unique;
    public double price;
    public string cat1;
    public string cat2;
    public string cat3;

    public BootpayItem() { }
}

public class BootpayPayload
{
    public string application_id;
    public string pg;
    public string method;
    public List<string> methods;
    public string name;
    public double price;
    public double tax_free;
    public string order_id;
    public bool use_order_id;
    public string account_expire_at;
    public bool show_agree_window;
    public string ux;
    public string paramJson;
    public string user_token;

    //public string boot_key;
    //public bool sms_use;

    public BootpayPayload()
    {
        this.ux = "PG_DIALOG";
        this.methods = new List<string>();
    }

    public string toJson()
    {
        return "{" + string.Format("\"application_id\":\"{0}\",\"pg\":\"{1}\",\"method\":\"{2}\",\"methods\":[{3}],\"name\":\"{4}\",\"price\":{5},\"tax_free\":{6},\"order_id\":\"{7}\",\"use_order_id\":{8},\"account_expire_at\":\"{9}\",\"show_agree_window\":{10},\"ux\":\"{11}\",\"params\":\"{12}\",\"user_token\":\"{13}\"",
            application_id,
            pg,
            method,
            getMethods(),
            //string.Join(",", methods.ToArray()),
            name,
            price,
            tax_free,
            order_id,
            use_order_id == true ? "true" : "false",
            account_expire_at,
            show_agree_window == true ? "true" : "false",
            ux,
            paramJson,
            user_token
            ) + "}";
    }

    String getMethods()
    {
        string result = "";

        foreach(string value in methods) {
            if (result.Length > 0) result += ",";
            result += "\"" + value + "\"";
        }

        return result;
    }
}

public class BootpayExtra
{
    public string start_at;
    public string end_at;
    public int expire_month;
    public bool vbank_result;
    public List<int> quotas;
    public string app_scheme;
    public string app_scheme_host;
    //public string ux;
    public string locale;
    public int popup;
    public int escrow;

    public BootpayExtra()
    {
        //this.ux = "PG_DIALOG";
        this.quotas = new List<int>();
    }
}