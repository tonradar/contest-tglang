#define _XOPEN_SOURCE 700
#include "tglang.h"

#include <stdio.h>
#include <string.h>
#include <stdlib.h>

#include <sys/time.h>
#include <assert.h>
#include <regex.h>
#include <onnxruntime_c_api.h>

const OrtApi *ortApi = NULL;
static OrtSession *session = NULL;
static int languegeMap[] = {2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 95, 96, 97, 98, 99};
void static_constructor(void) __attribute__((constructor));
const static int languageCount = 94;
const float threshhold = 0.8;

void static_constructor(void)
{
  /* do some constructing here … */
  const char *model_path = "./resources/model.onnx";
  ortApi = OrtGetApiBase()->GetApi(ORT_API_VERSION);

  /* Initialize the environment */
  OrtEnv *env;
  check_status(ortApi->CreateEnv(ORT_LOGGING_LEVEL_WARNING, "tgland", &env));

  /* Create the session options  */
  OrtSessionOptions *session_options;
  check_status(ortApi->CreateSessionOptions(&session_options));

  /* Load the model from a file */
  check_status(ortApi->CreateSession(env, model_path, session_options, &session));
}

enum TglangLanguage tglang_detect_programming_language(const char *text)
{
  /* Validate the model */
  OrtModelMetadata *OrtModelMetadata;
  OrtStatus *validation_status = ortApi->SessionGetModelMetadata(session, &OrtModelMetadata);

  if (validation_status != NULL)
  {
    printf("Model validation failed: %s\n", ortApi->GetErrorMessage(validation_status));
    exit(1);
  }

  /* Get the input and output names */
  OrtStatus *status;
  OrtAllocator *allocator;
  check_status(ortApi->GetAllocatorWithDefaultOptions(&allocator));

  char *input_name;
  check_status(ortApi->SessionGetInputName(session, 0, allocator, &input_name));

  char *output_name1;
  check_status(ortApi->SessionGetOutputName(session, 0, allocator, &output_name1));

  char *output_name2;
  check_status(ortApi->SessionGetOutputName(session, 1, allocator, &output_name2));

  OrtMemoryInfo *memory_info;
  check_status(ortApi->CreateCpuMemoryInfo(OrtArenaAllocator, OrtMemTypeDefault, &memory_info));

  int64_t input_shape[2] = {1, 1};
  const char *input_values[1] = {text};
  OrtValue *input_tensor;
  check_status(ortApi->CreateTensorAsOrtValue(allocator, input_shape, 2, ONNX_TENSOR_ELEMENT_DATA_TYPE_STRING, &input_tensor));
  check_status(ortApi->FillStringTensor(input_tensor, input_values, 1U));

  const char *input_names[] = {input_name};
  const char *output_names[] = {output_name1, output_name2};

  OrtValue *output_tensors[2] = {NULL, NULL};

  check_status(ortApi->Run(session, NULL, input_names, (const OrtValue *const *)&input_tensor, 1, output_names, 2, output_tensors));

  int64_t *label_data;

  check_status(ortApi->GetTensorMutableData(output_tensors[0], (void **)&label_data));

  float *probability_data;
  float probabilities[languageCount];
  int64_t out_shape[2] = {1, languageCount};
  check_status(ortApi->GetTensorMutableData(output_tensors[1], (void **)&probability_data));

  int i;

  for (i = 0; i < languageCount; i++)
  {
    probabilities[i] = probability_data[i];
  }

  enum TglangLanguage tglang = (enum TglangLanguage)label_data[0];

  int probabilityIndex = get_probability_index(tglang);
  float probability = probabilities[probabilityIndex];

  ortApi->ReleaseValue(input_tensor);
  for (i = 0; i < 2; i++)
  {
    ortApi->ReleaseValue(output_tensors[i]);
  }

  if (probability < threshhold)
  {
    return TGLANG_LANGUAGE_OTHER;
  }

  return tglang;
}

int get_probability_index(enum TglangLanguage tglang)
{
  int i;
  for (i = 0; i < 96; i++)
  {
    if (languegeMap[i] == (int)tglang)
    {
      return i;
    }
  }

  return -1;
}

void check_status(OrtStatus *status)
{
  if (status != NULL)
  {
    printf("check status");
    const char *msg = ortApi->GetErrorMessage(status);
    fprintf(stderr, "%s\n", msg);
    ortApi->ReleaseStatus(status);
    exit(1);
  }
}
