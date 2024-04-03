# 🤖 ASSISTENTE.DEV - Your Coding Companion

 [Website](https://assistente.dev) | [Nuget](https://www.nuget.org/packages/ASSISTENTE/#readme-body-tab)

---

![GitHub](https://img.shields.io/github/license/jarmatys/DEV_ASSISTENTE) ![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/jarmatys/DEV_ASSISTENTE/release-package.yml?label=release) ![Nuget](https://img.shields.io/nuget/v/ASSISTENTE?label=version) ![Nuget](https://img.shields.io/nuget/dt/ASSISTENTE) ![GitHub issues](https://img.shields.io/github/issues/jarmatys/DEV_ASSISTENTE) ![GitHub pull requests](https://img.shields.io/github/issues-pr/jarmatys/DEV_ASSISTENTE) 

---

![banner](ASSETS/banner.png)

---

Struggling to keep track of all your dev notes, bookmarks, and useful code snippets scattered across different platforms? Frustrated by the time wasted searching for that crucial information or starting from scratch? 🤔

Introducing `assistente.dev` – your ultimate assistant for developers. It's your personal memory bank, effortlessly storing and retrieving everything you need, precisely when you need it. Whether it's code snippets, course notes, or example solutions, Assistende has you covered. 🦾

Simply paste your query and watch as `assistente.dev` draws from your notes, bookmarks, and code bases to provide the perfect solution. With `assistente.dev`, say goodbye to wasted time and hello to seamless development. Get started and unlock your productivity potential! 🚀

---

### Roadmap

**Phases:**

- ✅ I phase (POC) - console app for test purpose `Playground`: [QUICK LINK](https://github.com/jarmatys/DEV_ASSISTENTE/tree/master/API/ASSISTENTE.Playground)
- 🔳 II phase - UI in Blazor + publish application on VPS
- 🔳 III phase - Improving prompts and logic to generate the most accurate answers

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
- 🔳 Add usage of MediatR and prepare Command & Queries libraries 
- 🔳 Generating answers or ready code (in console app) - depending on what the user needs
- 🔳 Add UI in Blazor
    - 🔳 Upload sample notes and code from `DEV_ASSISTENTE` repository for demo purpose
    - 🔳 Generating answers asynchronously (RabbitMQ + SignalR)
    - 🔳 Display resources list
    - 🔳 Display asked questions with answers list
- 🔳 Configure VPS to host all environment

#### Nice to have

- 🔳 Add library for translation (prompts and knowledge base)

#### Common tasks

- 🔳 Prepare video explainer with simply demo
- 🔳 Prepare `configuration` section in `Readme`
- 🔳 Prepare `quick start` section in `Readme`

---

### `WIP` Configuration

TBD

---

### `WIP` Quick start

TBD

---