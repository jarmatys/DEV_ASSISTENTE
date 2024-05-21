# 🤖 ASSISTENTE.DEV - Your Coding Companion

 🔗 QUICK LINKS: [APP Demo](https://app.assistente.dev) 🆕 | [Website](https://assistente.dev) | [Nuget](https://www.nuget.org/packages/ASSISTENTE/#readme-body-tab)

---

🐳 DOCKER IMAGES: [UI](https://hub.docker.com/repository/docker/armatysme/assistente-ui/general) | [API](https://hub.docker.com/repository/docker/armatysme/assistente-api/general) | [WORKER](https://hub.docker.com/repository/docker/armatysme/assistente-worker-sync/general) | [DB UPGRADER](https://hub.docker.com/repository/docker/armatysme/assistente-db-upgrade/general) |
 [PLAYGROUND](https://hub.docker.com/repository/docker/armatysme/assistente-playground/general)

---

![GitHub](https://img.shields.io/github/license/jarmatys/DEV_ASSISTENTE) ![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/jarmatys/DEV_ASSISTENTE/release-package.yml?label=release) ![Nuget](https://img.shields.io/nuget/v/ASSISTENTE?label=version) ![Nuget](https://img.shields.io/nuget/dt/ASSISTENTE) ![GitHub issues](https://img.shields.io/github/issues/jarmatys/DEV_ASSISTENTE) ![GitHub pull requests](https://img.shields.io/github/issues-pr/jarmatys/DEV_ASSISTENTE) 

---

![banner](https://raw.githubusercontent.com/jarmatys/DEV_ASSISTENTE/master/ASSETS/banner.png)

---

Struggling to keep track of all your dev notes, bookmarks, and useful code snippets scattered across different platforms? Frustrated by the time wasted searching for that crucial information or starting from scratch? 🤔

Introducing `assistente.dev` – your ultimate assistant for developers. It's your personal memory bank, effortlessly storing and retrieving everything you need, precisely when you need it. Whether it's code snippets, course notes, or example solutions, Assistende has you covered. 🦾

Simply paste your query and watch as `assistente.dev` draws from your notes, bookmarks, and code bases to provide the perfect solution. With `assistente.dev`, say goodbye to wasted time and hello to seamless development. Get started and unlock your productivity potential! 🚀

![assistente-preview](https://raw.githubusercontent.com/jarmatys/DEV_ASSISTENTE/master/ASSETS/assistente-preview.gif)

---

### Answer generation flow:

![answer-generation-flow](https://raw.githubusercontent.com/jarmatys/DEV_ASSISTENTE/master/ASSETS/answer-generation-flow.png)

---
### <img src="https://raw.githubusercontent.com/danielcranney/readme-generator/main/public/icons/socials/youtube.svg" width="18" height="16" alt="YouTube" /> Video Explainer

[![Assistente Video Explainer](https://img.youtube.com/vi/5l5J5WqOT2w/0.jpg)](https://www.youtube.com/watch?v=5l5J5WqOT2w)

---

### Roadmap

**Phases:**

- ✅ I phase (POC) - console app for test purpose `Playground`: [QUICK LINK](https://github.com/jarmatys/DEV_ASSISTENTE/tree/master/API/ASSISTENTE.Playground)
- ✅ II phase - UI in Blazor 
- ✅ III phase - Request limitation (throttling) + publish application on VPS - [URL](https://app.assistente.dev)
- 🔳 IV phase - Improve prompts and logic to generate the most accurate answers

#### Loading user data

**TODO:**

- ✅ Loading and parsing  `*.md` files - knowledge base
- ✅ Loading and parsing  `*.cs` files - code base
- ✅ Find open source solution for creating embeddins
    - ✅ Done partially (currently in use OpenAI embedding  service)
    - 🔳 `NICE TO HAVE` Integrate [LLamaSharp](https://github.com/SciSharp/LLamaSharp) for embeding creation
- ✅ Integration with Qdrant - saving embeddings

#### Generating results

- ✅ Generating prompt based on the 'knowledge base' & 'code base'
    - ✅ Generate prompt based on 'knowledge base'
    - ✅ Generate prompt based on 'code base'
- ✅ Integration with the OpenAI API
- ✅ Add usage of MediatR and prepare Command & Queries libraries 
- ✅ Generating answers or ready code (in console app) - depending on what the user needs
- ✅ Add UI in Blazor
    - ✅ Upload sample notes and code from `DEV_ASSISTENTE` repository for demo purpose
    - ✅ Generating answers asynchronously (RabbitMQ + SignalR)
    - ✅ Display resources list
    - ✅ Display asked questions with answers list
- ✅ Switch from MSSQL to PostgreSQL
- ✅ Configure VPS to host all environment
- ✅ Add support for separated scenario for `CODE` generation
    - ✅ Added table `QuestionFiles` to save information about select files based on question

#### Common tasks

- ✅ Prepare `configuration` section in `Readme`
- ✅ Prepare `quick start` section in `Readme`
- ✅ Prepare video explainer with simply demo

---
### Configuration

1. Fill out the settings file `appsettings.json` - [QUICK LINK](https://github.com/jarmatys/DEV_ASSISTENTE/blob/master/API/appsettings.json)
    
    - `OpenAI_ApiKey` - `<API_KEY>`

2. Fill out the `.env` file - [QUICK LINK](https://github.com/jarmatys/DEV_ASSISTENTE/blob/master/.env)

    - `OpenAI_ApiKey` - `<API_KEY>`

### Quick start

Prerequisites: `Docker` 

1. Fill out the settings file (`appsettings.json`) and (`.env`) from [CONFIGURATION](#Configuration) section

2. Run `start-enviroment.ps1` script with parameters:
    - Firstly with `-s` - to start required services
    - Secondly with `-d` - to upgrade and migrate database
    - Thirdly with `-a` - to start assistente application
    - Fourthly with `-p` - to learn assistente

4. Voila! Currently you can go to `https://localhost:1008`