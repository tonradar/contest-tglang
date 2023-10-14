# TonRadar Team's Solution for Telegram ML Competition 2023
This document explains how **TonRadar Team** developed a solution for the **ML Competition 2023** organized by Telegram. The goal of the competition is to create a library that can detect the programming language of a given source code snippet. The complete source code and documentation (including this document) are available on our GitHub repository: [github.com/tonradar/contest-tglang](^1^).

## Language Selection
We analyzed the list of languages provided by the competition and decided to exclude some of them for the following reasons:
 - Some languages are not widely used or not even real languages (e.g., **1S_ENTERPRISE**, **BISON**).
 - Some languages are very similar to other languages and can confuse the model (e.g., **Delphi** and **Pascal**).
 - Some languages have very few or no source code files available on GitHub (e.g., **ICON**, **LOGO**).

 Here is the list of excluded languages:
 - 1S_ENTERPRISE
 - BISON
 - DELPHI
 - FORTH
 - ICON
 - LOGO
 
We also chose to use `.aspx` files instead of `.asp` files for the `ASP` language, as `.aspx` is the newer and more popular framework.

 ## Data Collection
We used GitHub as our main source of data for training our model. We developed a project called **TgLang.CodeCrawler** that can crawl GitHub and download 1000 source code files for each language. The project is written in C# and consists of two parts:
  - **TgLang.CodeCrawler**: A console application that crawls GitHub and saves the files in a folder named after the language.
  - **TgLang.CodeCrawler.Test**: A set of unit tests and integration tests to ensure the correctness and reliability of the crawler.

  ## Model Training
We used Python and AzureML to train our model using the collected dataset. We chose a deep neural network architecture that can learn from the syntactic and semantic features of the source code. We saved our model in ONNX format so that we can use it in a C program to create the final library.

## Final Library
The final library is written in C and conforms to the specifications of the competition. It loads the ONNX model and uses it to predict the programming language of an input string. It returns the related enum of the predicted language as output.

