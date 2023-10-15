# This file has been autogenerated by version 1.52.0 of the Azure Automated Machine Learning SDK.


import numpy
import numpy as np
import pandas as pd
import pickle
import argparse


# For information on AzureML packages: https://docs.microsoft.com/en-us/python/api/?view=azure-ml-py
from azureml.training.tabular._diagnostics import logging_utilities


def setup_instrumentation(automl_run_id):
    import logging
    import sys

    from azureml.core import Run
    from azureml.telemetry import INSTRUMENTATION_KEY, get_telemetry_log_handler
    from azureml.telemetry._telemetry_formatter import ExceptionFormatter

    logger = logging.getLogger("azureml.training.tabular")

    try:
        logger.setLevel(logging.INFO)

        # Add logging to STDOUT
        stdout_handler = logging.StreamHandler(sys.stdout)
        logger.addHandler(stdout_handler)

        # Add telemetry logging with formatter to strip identifying info
        telemetry_handler = get_telemetry_log_handler(
            instrumentation_key=INSTRUMENTATION_KEY, component_name="azureml.training.tabular"
        )
        telemetry_handler.setFormatter(ExceptionFormatter())
        logger.addHandler(telemetry_handler)

        # Attach run IDs to logging info for correlation if running inside AzureML
        try:
            run = Run.get_context()
            return logging.LoggerAdapter(logger, extra={
                "properties": {
                    "codegen_run_id": run.id,
                    "automl_run_id": automl_run_id
                }
            })
        except Exception:
            pass
    except Exception:
        pass

    return logger


automl_run_id = 'ashy_bread_42r5bmrpb0_3'
logger = setup_instrumentation(automl_run_id)


def split_dataset(X, y, weights, split_ratio, should_stratify):
    '''
    Splits the dataset into a training and testing set.

    Splits the dataset using the given split ratio. The default ratio given is 0.25 but can be
    changed in the main function. If should_stratify is true the data will be split in a stratified
    way, meaning that each new set will have the same distribution of the target value as the
    original dataset. should_stratify is true for a classification run, false otherwise.
    '''
    from sklearn.model_selection import train_test_split

    random_state = 42
    if should_stratify:
        stratify = y
    else:
        stratify = None

    if weights is not None:
        X_train, X_test, y_train, y_test, weights_train, weights_test = train_test_split(
            X, y, weights, stratify=stratify, test_size=split_ratio, random_state=random_state
        )
    else:
        X_train, X_test, y_train, y_test = train_test_split(
            X, y, stratify=stratify, test_size=split_ratio, random_state=random_state
        )
        weights_train, weights_test = None, None

    return (X_train, y_train, weights_train), (X_test, y_test, weights_test)


def get_training_dataset(dataset_uri):
    
    from azureml.core.run import Run
    from azureml.data.abstract_dataset import AbstractDataset
    
    logger.info("Running get_training_dataset")
    ws = Run.get_context().experiment.workspace
    dataset = AbstractDataset._load(dataset_uri, ws)
    return dataset.to_pandas_dataframe()


def prepare_data(dataframe):
    '''
    Prepares data for training.
    
    Cleans the data, splits out the feature and sample weight columns and prepares the data for use in training.
    This function can vary depending on the type of dataset and the experiment task type: classification,
    regression, or time-series forecasting.
    '''
    
    from azureml.training.tabular.preprocessing import data_cleaning
    
    logger.info("Running prepare_data")
    label_column_name = 'LanguageCode'
    
    # extract the features, target and sample weight arrays
    y = dataframe[label_column_name].values
    X = dataframe.drop([label_column_name], axis=1)
    sample_weights = None
    X, y, sample_weights = data_cleaning._remove_nan_rows_in_X_y(X, y, sample_weights,
     is_timeseries=False, target_column=label_column_name)
    
    return X, y, sample_weights


def get_mapper_0(column_names):
    from azureml.training.tabular.featurization.text.stringcast_transformer import StringCastTransformer
    from numpy import float32
    from sklearn.feature_extraction.text import TfidfVectorizer
    from sklearn_pandas.dataframe_mapper import DataFrameMapper
    from sklearn_pandas.features_generator import gen_features
    
    definition = gen_features(
        columns=column_names,
        classes=[
            {
                'class': StringCastTransformer,
            },
            {
                'class': TfidfVectorizer,
                'analyzer': 'word',
                'binary': False,
                'decode_error': 'strict',
                'dtype': numpy.float32,
                'encoding': 'utf-8',
                'input': 'content',
                'lowercase': False,
                'max_df': 1.0,
                'max_features': None,
                'min_df': 1,
                'ngram_range': (1, 2),
                'norm': 'l2',
                'preprocessor': None,
                'smooth_idf': True,
                'stop_words': None,
                'strip_accents': None,
                'sublinear_tf': False,
                'token_pattern': '(?u)\\b\\w\\w+\\b',
                'tokenizer': None,
                'use_idf': False,
                'vocabulary': None,
            },
        ]
    )
    mapper = DataFrameMapper(features=definition, input_df=True, sparse=True)
    
    return mapper
    
    
def generate_data_transformation_config():
    '''
    Specifies the featurization step in the final scikit-learn pipeline.
    
    If you have many columns that need to have the same featurization/transformation applied (for example,
    50 columns in several column groups), these columns are handled by grouping based on type. Each column
    group then has a unique mapper applied to all columns in the group.
    '''
    from sklearn.pipeline import FeatureUnion
    
    column_group_0 = ['SourceCode']
    
    mapper = get_mapper_0(column_group_0)
    return mapper
    
    
def generate_preprocessor_config_0():
    '''
    Specifies a preprocessing step to be done after featurization in the final scikit-learn pipeline.
    
    Normally, this preprocessing step only consists of data standardization/normalization that is
    accomplished with sklearn.preprocessing. Automated ML only specifies a preprocessing step for
    non-ensemble classification and regression models.
    '''
    from sklearn.preprocessing import MaxAbsScaler
    
    preproc = MaxAbsScaler(
        copy=True
    )
    
    return preproc
    
    
def generate_algorithm_config_0():
    from lightgbm.sklearn import LGBMClassifier
    
    algorithm = LGBMClassifier(
        boosting_type='gbdt',
        class_weight=None,
        colsample_bytree=1.0,
        importance_type='split',
        learning_rate=0.1,
        max_depth=-1,
        min_child_samples=20,
        min_child_weight=0.001,
        min_split_gain=0.0,
        n_estimators=100,
        n_jobs=-1,
        num_leaves=31,
        objective=None,
        random_state=None,
        reg_alpha=0.0,
        reg_lambda=0.0,
        silent=True,
        subsample=1.0,
        subsample_for_bin=200000,
        subsample_freq=0,
        verbose=-10
    )
    
    return algorithm
    
    
def generate_preprocessor_config_1():
    from sklearn.preprocessing import MaxAbsScaler
    
    preproc = MaxAbsScaler(
        copy=True
    )
    
    return preproc
    
    
def generate_algorithm_config_1():
    from xgboost.sklearn import XGBClassifier
    
    algorithm = XGBClassifier(
        base_score=0.5,
        booster='gbtree',
        colsample_bylevel=1,
        colsample_bynode=1,
        colsample_bytree=1,
        gamma=0,
        gpu_id=-1,
        importance_type='gain',
        interaction_constraints='',
        learning_rate=0.300000012,
        max_delta_step=0,
        max_depth=6,
        min_child_weight=1,
        missing=numpy.nan,
        monotone_constraints='()',
        n_estimators=100,
        n_jobs=0,
        num_parallel_tree=1,
        objective='multi:softprob',
        random_state=0,
        reg_alpha=0,
        reg_lambda=1,
        scale_pos_weight=None,
        subsample=1,
        tree_method='auto',
        use_label_encoder=True,
        validate_parameters=1,
        verbose=-10,
        verbosity=0
    )
    
    return algorithm
    
    
def generate_algorithm_config():
    '''
    Specifies the actual algorithm and hyperparameters for training the model.
    
    It is the last stage of the final scikit-learn pipeline. For ensemble models, generate_preprocessor_config_N()
    (if needed) and generate_algorithm_config_N() are defined for each learner in the ensemble model,
    where N represents the placement of each learner in the ensemble model's list. For stack ensemble
    models, the meta learner generate_algorithm_config_meta() is defined.
    '''
    from azureml.training.tabular.models.voting_ensemble import PreFittedSoftVotingClassifier
    from numpy import array
    from sklearn.pipeline import Pipeline
    
    pipeline_0 = Pipeline(steps=[('preproc', generate_preprocessor_config_0()), ('model', generate_algorithm_config_0())])
    pipeline_1 = Pipeline(steps=[('preproc', generate_preprocessor_config_1()), ('model', generate_algorithm_config_1())])
    algorithm = PreFittedSoftVotingClassifier(
        classification_labels=numpy.array([ 2,  3,  4,  5,  6,  7,  9, 10, 11, 12, 13, 15, 16, 17, 18, 19, 20,
                                     21, 22, 23, 24, 26, 27, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39,
                                     40, 41, 43, 44, 45, 46, 48, 49, 51, 52, 53, 54, 55, 56, 57, 58, 59,
                                     60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76,
                                     78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 95,
                                     96, 97, 98, 99]),
        estimators=[
            ('model_0', pipeline_0),
            ('model_1', pipeline_1),
        ],
        flatten_transform=None,
        weights=[0.5, 0.5]
    )
    
    return algorithm
    
    
def build_model_pipeline():
    '''
    Defines the scikit-learn pipeline steps.
    '''
    from sklearn.pipeline import Pipeline
    
    logger.info("Running build_model_pipeline")
    pipeline = Pipeline(
        steps=[
            ('featurization', generate_data_transformation_config()),
            ('ensemble', generate_algorithm_config()),
        ]
    )
    
    return pipeline


def train_model(X, y, sample_weights=None, transformer=None):
    '''
    Calls the fit() method to train the model.
    
    The return value is the model fitted/trained on the input data.
    '''
    
    logger.info("Running train_model")
    model_pipeline = build_model_pipeline()
    
    model = model_pipeline.fit(X, y)
    return model


def calculate_metrics(model, X, y, sample_weights, X_test, y_test, cv_splits=None):
    '''
    Calculates the metrics that can be used to evaluate the model's performance.
    
    Metrics calculated vary depending on the experiment type. Classification, regression and time-series
    forecasting jobs each have their own set of metrics that are calculated.'''
    
    from azureml.training.tabular.score.scoring import score_classification
    
    y_pred_probs = model.predict_proba(X_test)
    if isinstance(y_pred_probs, pd.DataFrame):
        y_pred_probs = y_pred_probs.values
    class_labels = np.unique(y)
    train_labels = model.classes_
    metrics = score_classification(
        y_test, y_pred_probs, get_metrics_names(), class_labels, train_labels, use_binary=True)
    return metrics


def get_metrics_names():
    
    metrics_names = [
        'classification_report',
        'average_precision_score_micro',
        'recall_score_binary',
        'f1_score_micro',
        'average_precision_score_macro',
        'norm_macro_recall',
        'accuracy',
        'recall_score_micro',
        'AUC_binary',
        'AUC_weighted',
        'f1_score_binary',
        'f1_score_weighted',
        'accuracy_table',
        'precision_score_micro',
        'iou_classwise',
        'precision_score_classwise',
        'recall_score_classwise',
        'precision_score_binary',
        'precision_score_weighted',
        'AUC_macro',
        'confusion_matrix',
        'iou_micro',
        'log_loss',
        'iou_weighted',
        'average_precision_score_weighted',
        'balanced_accuracy',
        'iou',
        'weighted_accuracy',
        'AUC_micro',
        'AUC_classwise',
        'precision_score_macro',
        'recall_score_weighted',
        'recall_score_macro',
        'iou_macro',
        'average_precision_score_classwise',
        'average_precision_score_binary',
        'f1_score_classwise',
        'f1_score_macro',
        'matthews_correlation',
    ]
    return metrics_names


def get_metrics_log_methods():
    
    metrics_log_methods = {
        'classification_report': 'Skip',
        'average_precision_score_micro': 'log',
        'recall_score_binary': 'log',
        'f1_score_micro': 'log',
        'average_precision_score_macro': 'log',
        'norm_macro_recall': 'log',
        'accuracy': 'log',
        'recall_score_micro': 'log',
        'AUC_binary': 'log',
        'AUC_weighted': 'log',
        'f1_score_binary': 'log',
        'f1_score_weighted': 'log',
        'accuracy_table': 'log_accuracy_table',
        'precision_score_micro': 'log',
        'iou_classwise': 'Skip',
        'precision_score_classwise': 'Skip',
        'recall_score_classwise': 'Skip',
        'precision_score_binary': 'log',
        'precision_score_weighted': 'log',
        'AUC_macro': 'log',
        'confusion_matrix': 'log_confusion_matrix',
        'iou_micro': 'Skip',
        'log_loss': 'log',
        'iou_weighted': 'Skip',
        'average_precision_score_weighted': 'log',
        'balanced_accuracy': 'log',
        'iou': 'Skip',
        'weighted_accuracy': 'log',
        'AUC_micro': 'log',
        'AUC_classwise': 'Skip',
        'precision_score_macro': 'log',
        'recall_score_weighted': 'log',
        'recall_score_macro': 'log',
        'iou_macro': 'Skip',
        'average_precision_score_classwise': 'Skip',
        'average_precision_score_binary': 'log',
        'f1_score_classwise': 'Skip',
        'f1_score_macro': 'log',
        'matthews_correlation': 'log',
    }
    return metrics_log_methods


def main(training_dataset_uri=None):
    '''
    Runs all functions defined above.
    '''
    
    from azureml.automl.core.inference import inference
    from azureml.core.run import Run
    from azureml.training.tabular.score._cv_splits import _CVSplits
    from azureml.training.tabular.score.scoring import aggregate_scores
    
    import mlflow
    
    # The following code is for when running this code as part of an AzureML script run.
    run = Run.get_context()
    
    df = get_training_dataset(training_dataset_uri)
    X, y, sample_weights = prepare_data(df)
    cv_splits = _CVSplits(X, y, frac_valid=None, CV=3, n_step=None, is_time_series=False, task='classification')
    scores = []
    for X_train, y_train, sample_weights_train, X_valid, y_valid, sample_weights_valid in cv_splits.apply_CV_splits(X, y, sample_weights):
        partially_fitted_model = train_model(X_train, y_train, sample_weights_train)
        metrics = calculate_metrics(partially_fitted_model, X, y, sample_weights, X_test=X_valid, y_test=y_valid, cv_splits=cv_splits)
        scores.append(metrics)
        print(metrics)
    model = train_model(X_train, y_train, sample_weights_train)
    
    metrics = aggregate_scores(scores)
    metrics_log_methods = get_metrics_log_methods()
    print(metrics)
    for metric in metrics:
        if metrics_log_methods[metric] == 'None':
            logger.warning("Unsupported non-scalar metric {}. Will not log.".format(metric))
        elif metrics_log_methods[metric] == 'Skip':
            pass # Forecasting non-scalar metrics and unsupported classification metrics are not logged
        else:
            getattr(run, metrics_log_methods[metric])(metric, metrics[metric])
    cd = inference.get_conda_deps_as_dict(True)
    
    # Saving ML model to outputs/.
    signature = mlflow.models.signature.infer_signature(X, y)
    mlflow.sklearn.log_model(
        sk_model=model,
        artifact_path='outputs/',
        conda_env=cd,
        signature=signature,
        serialization_format=mlflow.sklearn.SERIALIZATION_FORMAT_PICKLE)
    
    run.upload_folder('outputs/', 'outputs/')


if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument('--training_dataset_uri', type=str, default='azureml://locations/westeurope/workspaces/a1eb233e-c0bf-4eee-bf28-6228d45e5e1f/data/TgCodeSamples_1/versions/14',     help='Default training dataset uri is populated from the parent run')
    args = parser.parse_args()
    
    try:
        main(args.training_dataset_uri)
    except Exception as e:
        logging_utilities.log_traceback(e, logger)
        raise