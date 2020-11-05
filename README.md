# FriendReminders Microservice Sample

This is a small Web API project that is used to demonstrate common techniques can be applied in the microservice design architecture. It manages a list reminders in an in-memory database with common actions such as Create/ Delete/Update Reminder Item. From here, wen can combine that service with others components such as Front-end web/mobile apps, Identity service and so on.

- Using .NET Core framework with some languages (C# for logic / testing implementation, and TyppeScript for infrastructure deployment).
- Using AWS Cloud Platform to run testing and deployment pipeline (AWS CodeBuild, AWS CodeDeploy and AWS CodePipeline)
- Deploying in ECS Cluster (EC2 or Fargate) with Rolling or Blue/Green Update approaches.

**Swagger UI:**

![Swagger UI](https://www.microservicesvn.com/assets/images/docs/middleware/swaggerui.png)

**AWS Pipeline:**

![AWS Deployment Pipeline](https://www.microservicesvn.com/assets/images/docs/cicd/cdk_codepipeline_execute.png)



