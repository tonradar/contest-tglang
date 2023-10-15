# TonRadar Solution for Telegram ML Competition 2023
This document explains how **TonRadar Team** developed a solution for the **ML Competition 2023** organized by Telegram. The goal of the competition is to create a library that can detect the programming language of a given source code snippet. The complete source code and documentation (including this document) are available on our GitHub repository: [github.com/tonradar/contest-tglang](^1^).

## Source Code
The source code is structured in these folders:
 - **[crawler](https://github.com/tonradar/contest-tglang/tree/main/src/crawler):** Crawling code files from GitHub (C#)
 - **[model](https://github.com/tonradar/contest-tglang/tree/main/src/model):** Training the model (Python, AzureML)
 - **[libtglang](https://github.com/tonradar/contest-tglang/tree/main/src/libtglang):** Building the final *.so* file (C)

## Language Selection
We analyzed the list of languages provided by the competition and decided to exclude some of them for the following reasons:
 - Some languages are not widely used or not even real languages (e.g., *1S_ENTERPRISE*, *BISON*).
 - Some languages are very similar to other languages and can confuse the model (e.g., *Delphi~Pascal* and *VISUAL_BASIC~VBSCRIPT*).
 - Some languages have very few or no source code files available on GitHub (e.g., *HACK*).

 Here is the list of **excluded** languages:
 - **1S_ENTERPRISE**
 - **BISON**
 - **HACK**
 - **DELPHI** (will map to PASCAL)
 - **VBSCRIPT** (will map to VISUAL_BASIC)
  
We also chose to use `.aspx` files instead of `.asp` files for the `ASP` language, as `.aspx` is the newer and more popular framework.

 ## Data Collection
We used GitHub as our main source of data for training our model. We developed a project called **TgLang.CodeCrawler** that can crawl GitHub and download 1000 source code files for each language. The project is written in C# and consists of two parts:
  - **TgLang.CodeCrawler**: A console application that crawls GitHub and saves the files in a folder named after the language.
  - **TgLang.CodeCrawler.Test**: A set of unit tests and integration tests to ensure the correctness and reliability of the crawler.

  ## Model Training
We used Python and AzureML to train our model using the collected dataset. We chose a deep neural network architecture that can learn from the syntactic and semantic features of the source code. We saved our model in ONNX format so that we could use it in a C program to create the final library.

We trained the model using AzureML and here is the result:
![Model Metrics](https://github.com/tonradar/contest-tglang/assets/5070766/bc169a40-ab77-480c-9997-9c1e9ba0c0fd)


During training, we observed that some languages caused confusion using the *Confusion Matrix*:
![Confusion Matrix](https://github.com/tonradar/contest-tglang/assets/5070766/eb391d21-f784-4fe9-82c2-08f411e63c0f)

As you can see in this confusion matrix, some blue columns show a high possibility of confusing most languages with the language of that column. 

These **confusing languages** are:
 - JSON
 - REGEX
 - CSV
 - INI

A common pattern in these languages is that they can be used within other languages. For example, a regex pattern or a JSON value can be used in any language. To fix this issue, if the model predicts one of these confusing languages, we will double-check the result using these functions: `is_json()`, `is_csv()`, `is_regex()`. It will be returned if it is valid, otherwise, the next probable language will be returned.
 - **is_regex()**: Checks if the string can be parsed to a regex.
 - **is_csv()**: Checks if the counts of commas for each line are the same
 - **is_json()**: Checks if the text is surrounded with `{}` or `[]`.

## Final Library
The final library is written in C and conforms to the specifications of the competition. It loads the ONNX model and uses it to predict the programming language of an input string. It returns the related enum of the predicted language as output.

