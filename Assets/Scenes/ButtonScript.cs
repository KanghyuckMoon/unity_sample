using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public BootpayViewObject bootpayViewObject;
    private bool iWantPay = true;

    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            //Destroy(bootpayViewObject); //더 이상 결제를 하지 않을때에만 Destory 수행
            return;
        }
    }

    public void StartBootpay()
    {
        Debug.Log(string.Format("Bootpay Webview Start"));


        BootpayUser bootpayUser = new BootpayUser(); // 구매자 정보 
        bootpayUser.username = "홍길동";
        bootpayUser.email = "testUser@email.com";
        bootpayUser.phone = "01012345678";
        bootpayUser.area = "서울";


        Debug.Log(string.Format("Bootpay Webview Start {0}", toJson(bootpayUser)));


        BootpayPayload bootpayPayload = new BootpayPayload(); // 결제 정보 
        bootpayPayload.price = 1000;
        bootpayPayload.name = "블링블링's 마스카라";
        bootpayPayload.order_id = "1234_1234_1243";
        bootpayPayload.ux = "PG_DIALOG";

#if UNITY_ANDROID         
        bootpayPayload.application_id = "5b8f6a4d396fa665fdc2b5e8"; //이곳에 android_application_id 를 넣어주세요 
#elif UNITY_IPHONE
        bootpayPayload.application_id = "5b8f6a4d396fa665fdc2b5e9"; //이곳에 ios_application_id 를 넣어주세요 
#endif
        bootpayPayload.pg = "danal"; // 이용하고자 하는 PG사, 없을 시 통합결제  
        bootpayPayload.method = "card"; // 이용하고자 하는 결제수단, 없을 시 통합결제 

        Debug.Log(string.Format("Bootpay Webview Start {0}", bootpayPayload.toJson()));

        BootpayExtra bootpayExtra = new BootpayExtra(); //그 외 결제 설정 정보 
        //bootpayExtra.escrow = 1;

        //bootpayPayload
        Debug.Log(string.Format("Bootpay Webview Start {0}", toJson(bootpayExtra)));

        List<string> items = new List<string>(); //구매하고자 하는 아이템 정보, 배열 형태로 사용  
        BootpayItem item1 = new BootpayItem();
        item1.item_name = "미\"키's 마우스";
        item1.qty = 1;
        item1.unique = "ITEM_CODE_MOUSE";
        item1.price = 1000;

        BootpayItem item2 = new BootpayItem();
        item2.item_name = "키보드";
        item2.qty = 1;
        item2.unique = "ITEM_CODE_KEYBOARD";
        item2.price = 1000;
        item2.cat1 = "패션";
        item2.cat2 = "여\"성'상의";
        item2.cat3 = "블라우스";

        items.Add(toJson(item1));
        items.Add(toJson(item2));

        Debug.Log(string.Format("Bootpay Webview Start {0}", "[" + string.Join(",", items.ToArray()) + "]"));


        bootpayViewObject =
            (new GameObject("BootpayViewObject")).AddComponent<BootpayViewObject>();
        bootpayViewObject.gameObject.name = "BootpayViewObject";
        bootpayViewObject.name = "BootpayViewObject";

        bootpayViewObject.Init(
            confirm: (data) => {
                Debug.Log(string.Format("CallOnConfirm[{0}]", data));

                if (iWantPay) // 재고가 있거나 결제하기를 원할 경우 
                    bootpayViewObject.TransactionConfirm(data);  //반드시 수행해야 결제가 최종 승인됨 
                else // 재고가 없거나, 결제를 승인하면 안될 경우 
                    bootpayViewObject.Dismiss();
            },
            done: (data) => {
                Debug.Log(string.Format("CallOnDone[{0}]", data));
            },
            cancel: (data) => {
                Debug.Log(string.Format("CallOnCancel[{0}]", data));
                //bootpayViewObject.Dismiss();
                //Destroy(bootpayViewObject);
            },
            ready: (data) => {
                Debug.Log(string.Format("CallOnReady[{0}]", data));
            },
            close: (data) => {
                Debug.Log(string.Format("CallOnClose[{0}]", data));
                bootpayViewObject.Dismiss();
                //Destroy(bootpayViewObject);
            });

        bootpayStart(bootpayViewObject, bootpayPayload, bootpayUser, items, bootpayExtra);
    }

    void bootpayStart(BootpayViewObject obj, BootpayPayload payload, BootpayUser user, List<string> items, BootpayExtra extra)
    {
        bool isOneStore = true;  //원스토어일 경우 true, 아니면 false 
        obj.Request(payload.toJson(), toJson(user), "[" + string.Join(",", items.ToArray()) + "]", toJson(extra), isOneStore);
    }



    string toJson(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    public void buttonClick() {
        StartBootpay();
    } 
}
