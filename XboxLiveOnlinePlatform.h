// Copyright (C) 2021 Nejcraft Do Not Redistribute


#pragma once

#include "Engine/Scripting/Script.h"
#include <OnlinePlatform/IOnlinePlatform.h>
#include <xsapi/services.h>


API_CLASS(NoSpawn) class ONLINEPLATFORM_API XboxLiveOnlinePlatform : public IOnlinePlatform
{
	DECLARE_SCRIPTING_TYPE_MINIMAL(XboxLiveOnlinePlatform)
public:

	bool Init() override;

	bool VerifyOwnership() override;

	IAchievementService* cachedAchievement = nullptr;

	IAchievementService* GetAchievementService() override;

	IFriendsService* GetFriendsService() override;

};

API_CLASS(NoSpawn) class ONLINEPLATFORM_API XboxLiveOnlineAchievementService : public IAchievementService
{
	DECLARE_SCRIPTING_TYPE_MINIMAL(XboxLiveOnlineAchievementService)
public:
	XboxLiveOnlineAchievementService(XboxLiveOnlinePlatform* parent);

	void SetAchievementProgress(StringView& identifier, float value) override;

	void SetAchievement(StringView& identifier, bool value) override;

	//API_FUNCTION() virtual Array<MonoString*> GetAchievements() = 0;

	float GetAchievementProgress(StringView& identifier) override;

	bool GetAchievement(StringView& identifier) override;
};