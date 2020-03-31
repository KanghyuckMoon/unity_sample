//
//  BootpayWebviewPlugin.h
//  BootpayWebviewUnity
//
//  Created by Taesup Yoon on 07/01/2020.
//  Copyright © 2020 Taesup Yoon. All rights reserved.
//

#import <Foundation/Foundation.h>
//#import <BootpayWebviewUnity-Swift.h>
//$(SWIFT_MODULE_NAME)-Swift.h
#import <UnityFramework/UnityFramework-Swift.h>
 

@interface BootpayWebviewPlugin : NSObject <BootpayRequestProtocol> {
    char *gameObject;
}

@end
 

typedef struct {
     char *user_id; // id
     char *username;
     char *email;
     int gender;
     char *birth;
     char *phone;
     char *area;
 } BPUser;

typedef struct {
    char *item_name;
    int qty;
    char *unique;
    double price;
    char *cat1;
    char *cat2;
    char *cat3;
} BPItem;

typedef struct {
    char *application_id;
    char *pg;
    char *method;
    char **methods;
    
    char *name;
    double price;
    double tax_free;
    
    char *order_id;
    bool use_order_id;
//    @objc public var params: [String: Any] = [:]
    
    char *account_expire_at;
    bool show_agree_window;
    
    char *boot_key;
    char *ux;
    bool sms_use;
} BPPayload;

typedef struct {
    char *start_at;
    char *end_at;
    int expire_month;
    bool vbank_result;
    int quotas[12];
    char *app_scheme;
    char *app_scheme_host;
    char *ux;
    char *locale;
    int popup;
} BPExtra;
