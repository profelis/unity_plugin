cmake_minimum_required(VERSION 3.6.0 FATAL_ERROR)
project(unity_plugin CXX)

include(cmake/apple.cmake)

if (NOT (${CMAKE_BUILD_TYPE} MATCHES Release))
    SET(CMAKE_BUILD_TYPE Debug)
endif ()
message(STATUS "Build type: ${CMAKE_BUILD_TYPE}")

if (NOT DEFINED TARGET_DIR)
    set(TARGET_DIR ${CMAKE_SOURCE_DIR}/../Assets/Plugins/unity_plugin)
endif ()
message(STATUS "Target dir: ${TARGET_DIR}")

set(TARGET_DIR_EDITOR ${TARGET_DIR}/Editor)
set(TARGET_DIR_ANDROID ${TARGET_DIR}/Android/libs/${ANDROID_ABI})
set(TARGET_DIR_IOS ${TARGET_DIR}/iOS)

# Set platform-dependent compilation defines matching C#
if (IOS)
    add_definitions(-DTARGET_OS_IPHONE)
elseif (ANDROID_NDK)
    add_definitions(-DTARGET_OS_ANDROID)
elseif (EDITOR)
    add_definitions(-DTARGET_OS_EDITOR)
    if (WIN32)
        add_definitions(-DTARGET_OS_EDITOR_WIN)
    elseif (${CMAKE_SYSTEM_NAME} MATCHES "Darwin")
        add_definitions(-DTARGET_OS_EDITOR_OSX)
    elseif (${CMAKE_SYSTEM_NAME} MATCHES "Linux")
        add_definitions(-DTARGET_OS_EDITOR_LINUX)
    endif ()
else ()
    add_definitions(-DTARGET_OS_STANDALONE)
    if (WIN32)
        add_definitions(-DTARGET_OS_WIN)
    elseif (${CMAKE_SYSTEM_NAME} MATCHES "Darwin")
        add_definitions(-DTARGET_OS_OSX)
    elseif (${CMAKE_SYSTEM_NAME} MATCHES "Linux")
        add_definitions(-DTARGET_OS_LINUX)
    endif ()
endif ()

# Set output path
if (ANDROID_NDK)
    set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${TARGET_DIR_ANDROID})
    set(CMAKE_LIBRARY_OUTPUT_DIRECTORY_DEBUG ${TARGET_DIR_ANDROID})
    set(CMAKE_LIBRARY_OUTPUT_DIRECTORY_RELEASE ${TARGET_DIR_ANDROID})
    set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY ${TARGET_DIR_ANDROID})
elseif (IOS)
    set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${TARGET_DIR_IOS})
    set(CMAKE_LIBRARY_OUTPUT_DIRECTORY_DEBUG ${TARGET_DIR_IOS})
    set(CMAKE_LIBRARY_OUTPUT_DIRECTORY_RELEASE ${TARGET_DIR_IOS})
    set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY ${TARGET_DIR_IOS})
elseif (WIN32 OR (${CMAKE_SYSTEM_NAME} MATCHES "Darwin") OR (${CMAKE_SYSTEM_NAME} MATCHES "Linux"))
    if (EDITOR)
        set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${TARGET_DIR_EDITOR})
        set(CMAKE_LIBRARY_OUTPUT_DIRECTORY_DEBUG ${TARGET_DIR_EDITOR})
        set(CMAKE_LIBRARY_OUTPUT_DIRECTORY_RELEASE ${TARGET_DIR_EDITOR})
        set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY ${TARGET_DIR_EDITOR})
    else ()
        set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${TARGET_DIR})
        set(CMAKE_LIBRARY_OUTPUT_DIRECTORY_DEBUG ${TARGET_DIR})
        set(CMAKE_LIBRARY_OUTPUT_DIRECTORY_RELEASE ${TARGET_DIR})
        set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY ${TARGET_DIR})
    endif ()
endif ()

# Set include directories
include_directories(
        ${CMAKE_SOURCE_DIR}/External/include
        ${CMAKE_SOURCE_DIR}/Source)

file(GLOB EXTERNAL_SOURCE_FILES ${CMAKE_SOURCE_DIR}/External/src/*.cpp)
file(GLOB EXTERNAL_HEADER_FILES ${CMAKE_SOURCE_DIR}/External/include/*.h)

file(GLOB PLUGIN_SOURCE_FILES ${CMAKE_SOURCE_DIR}/Source/*.cpp)
file(GLOB PLUGIN_HEADER_FILES ${CMAKE_SOURCE_DIR}/Source/*.h)
set(
        SOURCES
        ${EXTERNAL_SOURCE_FILES}
        ${EXTERNAL_HEADER_FILES}
        ${PLUGIN_SOURCE_FILES}
        ${PLUGIN_HEADER_FILES}
)

if (NOT ANDROID_NDK AND (IOS OR (${CMAKE_SYSTEM_NAME} MATCHES "Darwin")))
    file(GLOB OBJECTIVE_M_FILES ${CMAKE_SOURCE_DIR}/objective_c/*.m)
    file(GLOB OBJECTIVE_MM_FILES ${CMAKE_SOURCE_DIR}/objective_c/*.mm)
    file(GLOB OBJECTIVE_HEADER_FILES ${CMAKE_SOURCE_DIR}/objective_c/*.h)

    set(
            SOURCES
            ${OBJECTIVE_M_FILES}
            ${OBJECTIVE_MM_FILES}
            ${OBJECTIVE_HEADER_FILES}
            ${SOURCES}
    )
endif ()

if (SWIFT)
    # set(IOS_ARCH arm64)
    enable_language(Swift)
    set(CMAKE_Swift_LANGUAGE_VERSION 4.2)
    file(GLOB SWIFT_FILES ${CMAKE_SOURCE_DIR}/swift/*.swift)
    file(GLOB SWIFT_MM_FILES ${CMAKE_SOURCE_DIR}/swift/*.mm)
    file(GLOB SWIFT_H_FILES ${CMAKE_SOURCE_DIR}/swift/*.h)

    set(
            SOURCES
            ${SWIFT_FILES}
            ${SWIFT_MM_FILES}
            ${SWIFT_H_FILES}
            ${SOURCES}
    )
endif ()

if (IOS)
    add_library(${PROJECT_NAME} STATIC ${SOURCES})
    set_target_properties(${PROJECT_NAME} PROPERTIES BUNDLE TRUE)
    #    set_xcode_property(${PROJECT_NAME} ENABLE_BITCODE "NO")
elseif (ANDROID_NDK)
    add_library(${PROJECT_NAME} MODULE ${SOURCES})
elseif (${CMAKE_SYSTEM_NAME} MATCHES "Darwin")
    # Build a library. If on an Apple platform, build it in a bundle.
    add_library(${PROJECT_NAME} MODULE ${SOURCES})
    set_target_properties(${PROJECT_NAME} PROPERTIES BUNDLE TRUE)
else ()
    add_library(${PROJECT_NAME} MODULE ${SOURCES})
endif ()

if (NOT ANDROID_NDK AND (IOS OR (${CMAKE_SYSTEM_NAME} MATCHES "Darwin")))
    if (XCODE)
        set_target_properties(${PROJECT_NAME} PROPERTIES XCODE_ATTRIBUTE_CLANG_ENABLE_OBJC_ARC "YES")
    else ()
        ADD_OSX_FRAMEWORK(Foundation ${PROJECT_NAME})
        # Activate ARC - Automatic Reference Counting
        target_compile_options(${PROJECT_NAME} PUBLIC "-fobjc-arc")
    endif ()
endif ()

if (SWIFT)
    if (XCODE)
        if (IOS)
            # force setup arch and deploy target for swift module
            set_target_properties(${PROJECT_NAME} PROPERTIES XCODE_ATTRIBUTE_IPHONEOS_DEPLOYMENT_TARGET ${IOS_DEPLOYMENT_TARGET})
            set_target_properties(${PROJECT_NAME} PROPERTIES XCODE_ATTRIBUTE_ARCHS "arm64 armv7")
        endif ()

        set_target_properties(${PROJECT_NAME} PROPERTIES XCODE_ATTRIBUTE_ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES "YES")
        set_target_properties(${PROJECT_NAME} PROPERTIES XCODE_ATTRIBUTE_LD_RUNPATH_SEARCH_PATHS "@loader_path/../Frameworks")
    endif ()
endif ()

# Enable C++11
set_property(TARGET ${PROJECT_NAME} PROPERTY CXX_STANDARD 11)