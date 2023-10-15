cmake_minimum_required(VERSION 3.5 FATAL_ERROR)

project(tglang VERSION 1.0.0 LANGUAGES C)

add_library(tglang SHARED tglang.c)

set(ONNXRUNTIME_LIB_PATH "/home/afshin/Desktop/tglang/libtglang/onnxruntime-linux-x64-1.16.1/lib")
set(ONNXRUNTIME_INCLUDE_PATH "/home/afshin/Desktop/tglang/libtglang/onnxruntime-linux-x64-1.16.1/include")

include_directories("${ONNXRUNTIME_INCLUDE_PATH}")

link_directories(${ONNXRUNTIME_LIB_PATH})

target_include_directories(tglang PRIVATE "${ONNXRUNTIME_INCLUDE_PATH}")
target_link_libraries(tglang PRIVATE "${ONNXRUNTIME_LIB_PATH}/libonnxruntime.so")