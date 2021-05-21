// Copyright (C) 2021 Nejcraft Do Not Redistribute

#include "XboxLiveOnlinePlatform.h"


bool XboxLiveOnlinePlatform::Init()
{
    return false;
}

bool XboxLiveOnlinePlatform::VerifyOwnership()
{
    return false;
}

IAchievementService* XboxLiveOnlinePlatform::GetAchievementService()
{
    if (cachedAchievement == nullptr)
    {
        cachedAchievement = New<XboxLiveOnlineAchievementService>(this);
    }
    return cachedAchievement;    
}

IFriendsService* XboxLiveOnlinePlatform::GetFriendsService()
{
    return nullptr;
}

XboxLiveOnlineAchievementService::XboxLiveOnlineAchievementService(XboxLiveOnlinePlatform* parent) {

}

void XboxLiveOnlineAchievementService::SetAchievementProgress(StringView& identifier, float value)
{
    return;
}

void XboxLiveOnlineAchievementService::SetAchievement(StringView& identifier, bool value)
{
    return;
}

float XboxLiveOnlineAchievementService::GetAchievementProgress(StringView& identifier)
{
    return 0;
}

bool XboxLiveOnlineAchievementService::GetAchievement(StringView& identifier)
{
    return false;
}