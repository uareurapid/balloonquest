//
//  BatchPlugin.m
//  iosExtension
//
//  http://batch.com
//  Copyright (c) 2014 Batch SDK. All rights reserved.
//

#import "BatchPlugin.h"
#import <BatchBridge/BatchJSONHelper.h>
#import <BatchBridge/BatchBridge.h>
#import <BatchBridge/BatchUserProfile.h>

extern void UnitySendMessage(const char *, const char *, const char *);

#pragma mark -
#pragma mark BatchAdsDelegate

@implementation BatchUnityInterstitial

+ (instancetype)unityInterstitialForHandle:(NSInteger)handle
{
    return [[BatchUnityInterstitial alloc] initWithHandle:handle shouldRetainSelf:YES];
}

- (instancetype)initWithHandle:(NSInteger)handle shouldRetainSelf:(BOOL)retain
{
    self = [super init];
    if (self)
    {
        self.handle = @(handle);
        self.selfRetained = retain;
        if (self.selfRetained)
        {
            CFBridgingRetain(self);
        }
    }
    return self;
}

#pragma mark Private Methods

- (void)sendUnityAdMessage:(NSString*)event
{
    NSString *jsonMessage = [BatchJSONHelper JSONStringFromObject:@{@"handle": self.handle,
                                                                    @"event": event}];
    
    const char *message = [jsonMessage cStringUsingEncoding:NSUTF8StringEncoding];
    
    UnitySendMessage("BatchUnityPlugin", "onAdDisplayListenerEvent", message==NULL?"":message);
}

#pragma mark BatchDisplayAdsDelegate methods

- (void)adDidAppear:(NSString*)placement
{
    [self sendUnityAdMessage:@"AdDisplayed"];
}

- (void)adDidDisappear:(NSString*)placement
{
    [self sendUnityAdMessage:@"AdClosed"];
    if (self.selfRetained)
    {
        CFRelease((__bridge CFTypeRef)self);
    }
}

- (void)adClicked:(NSString*)placement
{
    [self sendUnityAdMessage:@"AdClicked"];
}

- (void)adCancelled:(NSString*)placement
{
    [self sendUnityAdMessage:@"AdCancelled"];
}

- (void)adNotDisplayed:(NSString*)placement
{
    [self sendUnityAdMessage:@"NoAdDisplayed"];
    if (self.selfRetained)
    {
        CFRelease((__bridge CFTypeRef)self);
    }
}

@end

#pragma mark -
#pragma mark BatchPlugin

@interface BatchPlugin ()
{
    BatchPluginCallback *_callback;
}

@end

@implementation BatchPlugin

#pragma mark -
#pragma mark Instance methods

+ (void)load
{
    // Make sure that Batch is created early
    [BatchPlugin plugin];
}

// Singleton accessor.
+ (BatchPlugin *)plugin
{
    static BatchPlugin *sharedInstance = nil;
    static dispatch_once_t onceToken;
    
    dispatch_once(&onceToken, ^{
        
        sharedInstance = [[BatchPlugin alloc] init];
    });
    
	return sharedInstance;
}

- (id)init
{
    self = [super init];
    if (!self)
    {
        return self;
    }
    
    _callback = [BatchPluginCallback new];
    
    UnityRegisterAppDelegateListener(self);

    // Setup plugin version.
    NSString *infos = [NSString stringWithFormat:@"Unity/%@",PluginVersion];
    setenv("BATCH_PLUGIN_VERSION", [infos cStringUsingEncoding:NSUTF8StringEncoding], 1);
    
    return self;
}


#pragma mark -
#pragma mark Plugin methods

// Send an action to Batch.
- (NSString *)call:(NSString *)action withParameters:(NSString *)params
{
    NSDictionary *parameters = nil;
    
    if (params && params.length>0)
    {
        parameters = (NSDictionary *)[BatchJSONHelper objectFromJSONString:params];
    }
    
    return [BatchBridge call:action withParameters:parameters callback:_callback];
}


#pragma mark -
#pragma mark AppDelegateListener

- (void)onOpenURL:(NSNotification *)notification
{
    id url = [notification.userInfo objectForKey:@"url"];
    if ([url respondsToSelector:@selector(absoluteString)])
    {
        // The "url" must be a string.
        [BatchBridge call:REDEEM_URL withParameters:@{@"url": [url absoluteString]} callback:_callback];
    }
    else
    {
        NSLog(@"[BatchPlugin] Invalid URL: %@", [url description]);
    }
}

@end


#pragma mark -
#pragma mark BatchPluginCallback

@implementation BatchPluginCallback

// Callback from any actions.
- (void)call:(NSString *)actionString withResult:(id<NSSecureCoding>)result
{
    NSString *jsonMessage = [BatchJSONHelper JSONStringFromObject:result];
    
    const char *message = [jsonMessage cStringUsingEncoding:NSUTF8StringEncoding];
    const char *action = [actionString cStringUsingEncoding:NSUTF8StringEncoding];

//    NSLog(@"[UnityBatchPlugin] Sending action [%s] to Unity [%s]", action, message);
    
    UnitySendMessage("BatchUnityPlugin", action, message==NULL?"":message);
}

@end


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark -
#pragma mark extern "C"

// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

extern "C"
{
    const char* _call(const char* action, const char* parameter)
    {
        if (action)
        {
            NSString *a = [[NSString alloc] initWithUTF8String:action];
            NSString *p = [[NSString alloc] initWithUTF8String:parameter];
            
            NSString *result = [[BatchPlugin plugin] call:a withParameters:p];
            
#if !__has_feature(objc_arc)
            [a release];
            [p release];
#endif

            if (!result)
            {
                result = @"";
            }
            
            return [result cStringUsingEncoding:NSUTF8StringEncoding];
        }
        else
        {
            NSLog(@"[BatchPlugin] Null action string.");
        }
        
        return [@"" cStringUsingEncoding:NSUTF8StringEncoding];
    }
    
    void BAUnityInterstitialShow(const NSInteger gchandle, const char* placement)
    {
        if (placement)
        {
            NSString *p = [[NSString alloc] initWithUTF8String:placement];
            
            NSDictionary *parameters = @{@"placement": p, @"listener": [BatchUnityInterstitial unityInterstitialForHandle:gchandle]};

#if !__has_feature(objc_arc)
            [parameters retain];
#endif
            
            // We could use the callback and not send a listener. It's that way (for now)
            // in order to match the Java bridge.
            [BatchBridge call:ADS_DISPLAY_INTER_WITH_LISTENER withParameters:parameters callback:nil];

#if !__has_feature(objc_arc)
            [p release];
            [parameters release];
#endif
        }
        else
        {
            NSLog(@"[BatchPlugin] Null moment string.");
        }
    }
}
