#include "tglang.h"

#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <onnxruntime_c_api.h>
#include <sys/time.h>
#include <assert.h>

const OrtApi *ortApi = NULL;
static const OrtSession *session = NULL;
void static_constructor(void) __attribute__((constructor));

void static_constructor(void)
{
  /* do some constructing here … */
 // printf("start static constructor\n");

  const char *model_path = "./resources/model.onnx";

  ortApi = OrtGetApiBase()->GetApi(ORT_API_VERSION);

  // Initialize the environment
  OrtEnv *env;
  check_status(ortApi->CreateEnv(ORT_LOGGING_LEVEL_WARNING, "tgland", &env));

  // Create the session options
  OrtSessionOptions *session_options;
  check_status(ortApi->CreateSessionOptions(&session_options));

  // Load the model from a file

  check_status(ortApi->CreateSession(env, model_path, session_options, &session));

  //printf("end static constructor\n");
}

enum TglangLanguage tglang_detect_programming_language(const char *text)
{

  // Validate the model
  OrtModelMetadata *OrtModelMetadata;
  OrtStatus *validation_status = ortApi->SessionGetModelMetadata(session, &OrtModelMetadata);

  if (validation_status != NULL)
  {
    printf("Model validation failed: %s\n", ortApi->GetErrorMessage(validation_status));
    exit(1);
  }

  // Get the input and output names
  OrtStatus *status;
  OrtAllocator *allocator;
  check_status(ortApi->GetAllocatorWithDefaultOptions(&allocator));

  char *input_name;
  check_status(ortApi->SessionGetInputName(session, 0, allocator, &input_name));
  // fprintf(stderr, input_name);
  // fprintf(stderr, "\n");

  char *output_name1;
  check_status(ortApi->SessionGetOutputName(session, 0, allocator, &output_name1));

  // fprintf(stderr, output_name1);
  // printf("\n");

  char *output_name2;
  check_status(ortApi->SessionGetOutputName(session, 1, allocator, &output_name2));

  // fprintf(stderr, output_name2);
  // printf("\n");

  OrtMemoryInfo *memory_info;
  check_status(ortApi->CreateCpuMemoryInfo(OrtArenaAllocator, OrtMemTypeDefault, &memory_info));

  int64_t input_shape[2] = {1, 1};

  // char *input_values[1] = {"using a;"};
  const char *input_values[1] = {text};

  OrtValue *input_tensor;
  check_status(ortApi->CreateTensorAsOrtValue(allocator, input_shape, 2, ONNX_TENSOR_ELEMENT_DATA_TYPE_STRING, &input_tensor));
  check_status(ortApi->FillStringTensor(input_tensor, input_values, 1U));

  // Run the model
  const char *input_names[] = {input_name};
  const char *output_names[] = {output_name1, output_name2};

  OrtValue *output_tensors[2] = {NULL, NULL};

  check_status(ortApi->Run(session, NULL, input_names, (const OrtValue *const *)&input_tensor, 1, output_names, 2, output_tensors));

  int64_t *label_data;

  check_status(ortApi->GetTensorMutableData(output_tensors[0], (void **)&label_data));
 // printf("language code: %ld \n", (long)label_data[0]);
 // printf("\n");

  float *probabity_data;
  int64_t out_shape[2] = {1, 89};
  check_status(ortApi->GetTensorMutableData(output_tensors[1], (void **)&probabity_data));
  // Print the data
  for (int i = 0; i < out_shape[0]; i++)
  {
    for (int j = 0; j < out_shape[1]; j++)
    {

      // printf("%.6f \n", probabity_data[i * input_shape[1] + j]);
    }
    // printf("\n");
  }

  enum TglangLanguage tglang = (enum TglangLanguage)label_data[0];

  ortApi->ReleaseValue(input_tensor);
  for (int i = 0; i < 2; i++)
  {
    ortApi->ReleaseValue(output_tensors[i]);
  }

  return tglang;
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
