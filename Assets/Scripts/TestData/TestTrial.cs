﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class collects all data required to create a TestTrialDTO. 
/// Data includes trial number, start time, the target's angle, time to fixate on and activate target,
/// target center error, total head and cursor movement throughout trial, and whether a timeout or error occured.
/// TestTrials are visually represented by the active target during the sequence.
/// As such, the class is also in charge of the creation of DataLogs and DataLogDTOs.
/// </summary>

public class TestTrial
{
    // Pre-defined data.
    public float TargetAngle;

    // TestSequence result data.
    public DateTime StartTime;
    public int TrialNumber;
    public double TimeToFixate;
    public double TimeToActivate;
    public Vector2 TargetCenterError;
    public float TotalHeadMovement;
    public float TotalCursorMovement;
    public bool TimedOut;
    public bool Error;

    public List<DataLog> Logs;

    /// <summary>
    /// TestTrial constructor. 
    /// </summary>
    /// <param name="targetAngle"> Modified angle at which the target should spawn.</param>
    public TestTrial(float targetAngle)
    {
        TargetAngle = targetAngle;
        Logs = new List<DataLog>();
    }

    /// <summary>
    /// Effective amplitude calculation.
    /// </summary>
    /// <param name="from"> Position on the canvas of the center of the previous target.</param>
    /// <param name="to"> Position on the canvas of the center of the current target.</param>
    /// <param name="select"> Position on the canvas where the activation of the current target happened.</param>
    /// <returns></returns>
    public double CalculateTrialAe(Vector2 from, Vector2 to, Vector2 select) {
        double a = TestDataHelper.Hypotenuse(to.x - from.x, to.y - from.y);
        double dx = (select - to).magnitude;
        return a + dx;
    }

    /// <summary>
    /// Add new DataLog to a list of DataLogs.
    /// </summary>
    /// <param name="msProgress"> Milliseconds since start of the trial.</param>
    /// <param name="cursorPosition"> Position on the canvas of the cursor.</param>
    /// <param name="gazePosition"> Position on the canvas of the gaze.</param>
    /// <param name="pupilDiameterLeft"> Pupil diameter of the left eye. (PupilLabs only)</param>
    /// <param name="pupilDiameterRight"> Pupil diameter of the right eye. (PupilLabs only)</param>
    /// <param name="pupilDiameter3DLeft"> 3d Pupil diameter of the left eye. (PupilLabs only)</param>
    /// <param name="pupilDiameter3DRight"> 3d Pupil diameter of the right eye. (PupilLabs only)</param>
    /// <param name="pupilConfidenceLeft"> Pupil confidence of the left eye. (PupilLabs only)</param>
    /// <param name="pupilConfidenceRight"> Pupil confidence of the right eye. (PupilLabs only)</param>
    /// <param name="headMovement"> Sum of angular head movement up to this DataLog.</param>
    /// <param name="nosePosition"> Position on the canvas to which the center of the head (nose) is pointing.</param>
    /// <param name="hmdPositionX"> X coordinate position of the hmd.</param>
    /// <param name="hmdPositionY"> Y coordinate position of the hmd.</param>
    /// <param name="hmdPositionZ"> Z coordinate position of the hmd.</param>
    /// <param name="hmdRotationX"> X coordinate rotation of the hmd.</param>
    /// <param name="hmdRotationY"> Y coordinate rotation of the hmd.</param>
    /// <param name="hmdRotationZ"> Z coordinate rotation of the hmd.</param>
    public void LogData(double msProgress, Vector2 cursorPosition, Vector2 gazePosition, float? pupilDiameterLeft, float? pupilDiameterRight, 
        float? pupilDiameter3DLeft, float? pupilDiameter3DRight, float? pupilConfidenceLeft, float? pupilConfidenceRight, float? pupilTimestampLeft, float? pupilTimestampRight,
        float headMovement, Vector2 nosePosition, float? hmdPositionX, float? hmdPositionY, float? hmdPositionZ, float? hmdRotationX, float? hmdRotationY, float? hmdRotationZ)
    {
        Logs.Add(new DataLog(msProgress, cursorPosition, gazePosition, pupilDiameterLeft, pupilDiameterRight, pupilDiameter3DLeft, pupilDiameter3DRight,
            pupilConfidenceLeft, pupilConfidenceRight, pupilTimestampLeft, pupilTimestampRight, headMovement, nosePosition, hmdPositionX, hmdPositionY, hmdPositionZ,
            hmdRotationX, hmdRotationY, hmdRotationZ));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="testSequenceId"> The Id of the sequence that this trial is a part of.</param>
    /// <returns>Returns a TestTrialDTO.</returns>
    public TestTrialDTO CreateDTO(int testSequenceId)
    {
       return new TestTrialDTO(testSequenceId, TrialNumber, StartTime, TargetAngle, TimeToFixate, TimeToActivate, 
            TargetCenterError.x, TargetCenterError.y, TotalHeadMovement, TotalCursorMovement, TimedOut, Error);
    }

    /// <summary>
    /// Transform the list of DataLogs to a list of DataLogDTOs. 
    /// </summary>
    /// <param name="testTrialId"> The Id of the trial that these DataLogs are a part of.</param>
    /// <returns></returns>
    public List<DataLogDTO> GenerateDataLogDTOList(int testTrialId) {
        List<DataLogDTO> dtoLogs = new List<DataLogDTO>();
        foreach (DataLog dataLog in Logs) {
            dtoLogs.Add(dataLog.CreateDTO(testTrialId));
        }
        return dtoLogs;
    }

    /// <summary>
    /// Adjust TargetAngle prior to TestTrialDTO creation. 
    /// </summary>
    /// <param name="finalAngle"></param>
    public void UpdateTrialAngleForLogging(float finalAngle)
    {
        TargetAngle = finalAngle;
    }

    public override string ToString()
    {
        string s = "StartTime: " + TrialNumber + ": TimeToFixate " + TimeToFixate + ", TimeToActivate " + TimeToActivate +
                   ", TargetCenterError " + TargetCenterError + ", TotalHeadMovement " + TotalHeadMovement +
                   "\b, TotalCursorMovement " + TotalCursorMovement;
        return s;
    }

}