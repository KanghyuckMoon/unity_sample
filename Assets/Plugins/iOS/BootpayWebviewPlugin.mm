//
//  BootpayWebviewUnityProtocol.m
//  BootpayWebviewUnity
//
//  Created by Taesup Yoon on 07/01/2020.
//  Copyright © 2020 Taesup Yoon. All rights reserved.
//
 
#import "BootpayWebviewPlugin.h"


@implementation BootpayWebviewPlugin

- (void) onError:(NSDictionary<NSString *,id> * _Nonnull) data {
    UnitySendMessage(gameObject, "CallOnError", [self getJson: data]);
}

- (void) onReady:(NSDictionary<NSString *,id> * _Nonnull) data {
    UnitySendMessage(gameObject, "CallOnReady", [self getJson: data]);
}

- (void) onClose {
    UnitySendMessage(gameObject, "CallOnClose", "");
}

- (void) onConfirm:(NSDictionary<NSString *,id> * _Nonnull) data {
    UnitySendMessage(gameObject, "CallOnConfirm", [self getJson: data]);
}

- (void) onCancel:(NSDictionary<NSString *,id> * _Nonnull) data {
    UnitySendMessage(gameObject, "CallOnCancel", [self getJson: data]);
}

- (void) onDone:(NSDictionary<NSString *,id> * _Nonnull) data {
    UnitySendMessage(gameObject, "CallOnDone", [self getJson: data]);
}

//- (id)initWithGameObjectName:(const char *)gameObjectName_ transparent:(BOOL)transparent ua:(const char *)ua enableWKWebView:(BOOL)enableWKWebView
//{
//
//}

- (void) TransactionConfirm: (const char*) data {
    NSString *str = [NSString stringWithCString:data encoding: NSUTF8StringEncoding];
    NSData *jsonData = [str dataUsingEncoding: NSUTF8StringEncoding];
    NSError *e;
    NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:jsonData options:kNilOptions error:&e];
    [Bootpay transactionConfirm: dict];
}

- (void) RemovePaymentWindow {
    [Bootpay removePaymentWindow];
}

- (void) Dismiss {
    [Bootpay dismiss];
}

- (void) Request: (char *)gameObjectName payload:(const char *)payload user:(const char *)user items:(const char *)items extra:(const char *)extra isOneStore:(BOOL)isOneStore {
    gameObject = new char[sizeof(gameObjectName)];
    strcpy (gameObject, gameObjectName);
       
    UIViewController *rootViewController = UnityGetGLViewController();
//    BootpayWebviewPlugin *callback = [[BootpayWebviewPlugin alloc] init];
        
    
    [Bootpay request_objc_json:rootViewController
                            :self
                            :[NSString stringWithUTF8String: payload]
                            :[NSString stringWithUTF8String: user]
                            :[NSString stringWithUTF8String: items]
                            :[NSString stringWithUTF8String: extra]
                            :isOneStore
                            :[NSString stringWithUTF8String: gameObject]
     ];
}

- (const char *) getJson: (NSDictionary<NSString *,id> * _Nonnull) data {
    NSData *requestData = [NSJSONSerialization dataWithJSONObject:data options:0 error:nil];
    NSString *requestJson = [[NSString alloc] initWithData: requestData
                                              encoding:NSUTF8StringEncoding];
    return [requestJson UTF8String];
}
 
@end


#pragma mark - String Helpers

static NSString * const NSStringFromCString(const char *string)
{
    if (string != NULL) {
        return [NSString stringWithUTF8String:string];
    } else {
        return nil;
    }
}

#pragma mark - C interface

extern "C" {
    
    void _CBootpayWebviewPlugin_TransactionConfirm(void *instance, const char* data) {
        
        BootpayWebviewPlugin *webViewPlugin = (__bridge_transfer BootpayWebviewPlugin *)instance;
        [webViewPlugin TransactionConfirm:data];
    }
    
    void _CBootpayWebviewPlugin_RemovePaymentWindow(void *instance) {
        BootpayWebviewPlugin *webViewPlugin = (__bridge_transfer BootpayWebviewPlugin *)instance;
        [webViewPlugin RemovePaymentWindow];
    }
    
    void _CBootpayWebviewPlugin_Dismiss(void *instance) {
        BootpayWebviewPlugin *webViewPlugin = (__bridge_transfer BootpayWebviewPlugin *)instance;
        [webViewPlugin Dismiss];
        webViewPlugin = nil;
    }
    
//    void __IOS_Request(BPPayload payload, BPUser user, BPItem items[], BPExtra extra) {

//const char * value = json string
    void *_CBootpayWebviewPlugin_Request(char * gameObject, const char * payload, const char * user, const char * items, const char * extra, BOOL isOneStore) {
        BootpayWebviewPlugin *webviewPlugin = [[BootpayWebviewPlugin alloc] init];
        [webviewPlugin Request:gameObject payload:payload user:user items:items extra:extra isOneStore:isOneStore];
        return (__bridge_retained void *)webviewPlugin;
    }
}
