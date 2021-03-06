' NX 11.0.0.33
' Journal created by sunnyi on Thu May 03 13:50:27 2018 W. Europe Summer Time
'
Option Strict Off  
Imports System
Imports NXOpen
Imports System.IO 
Imports NXOpen.UF  

Module NXJournal
Sub Main (ByVal args() As String) 

Dim theSession As NXOpen.Session = NXOpen.Session.GetSession()
Dim workSimPart As NXOpen.CAE.SimPart = CType(theSession.Parts.BaseWork, NXOpen.CAE.SimPart)

Dim displaySimPart As NXOpen.CAE.SimPart = CType(theSession.Parts.BaseDisplay, NXOpen.CAE.SimPart)

Dim resultParameters1 As NXOpen.CAE.ResultParameters = Nothing
resultParameters1 = theSession.ResultManager.CreateResultParameters()

resultParameters1.SetDBScaling(0)

resultParameters1.SetDBReference(1.0)

resultParameters1.SetDBscale(NXOpen.CAE.Result.DbScale.Db10)

Dim resultManager1 As NXOpen.CAE.ResultManager = CType(theSession.ResultManager, NXOpen.CAE.ResultManager)

Dim solutionResult1 As NXOpen.CAE.SolutionResult = CType(resultManager1.FindObject("SolutionResult[simulation.sim_frequency]"), NXOpen.CAE.SolutionResult)

Dim loadcase1 As NXOpen.CAE.Loadcase = CType(solutionResult1.Find("Loadcase[1]"), NXOpen.CAE.Loadcase)

'Define loop
Dim i As Integer

For i = 1 To 88
    'Define eigenfrequency string, "Iteration[1]" yields first eigenfrequency, etc
    Dim iteration_string As String = "Iteration[" & cstr(i) & "]"

    Dim iteration1 As NXOpen.CAE.Iteration = CType(loadcase1.Find(iteration_string), NXOpen.CAE.Iteration)

    Dim resultType1 As NXOpen.CAE.ResultType = CType(iteration1.Find("ResultType[[Displacement][Nodal]]"), NXOpen.CAE.ResultType)

    resultParameters1.SetGenericResultType(resultType1)

    resultParameters1.SetResultBeamSection(-1)

    resultParameters1.SetResultShellSection(-1)

    resultParameters1.SetResultComponent(NXOpen.CAE.Result.Component.Magnitude)

    resultParameters1.SetCoordinateSystem(NXOpen.CAE.Result.CoordinateSystem.AbsoluteRectangular)

    resultParameters1.SetSelectedCoordinateSystem(NXOpen.CAE.Result.CoordinateSystemSource.None, -1)

    resultParameters1.SetRotationAxisOfAbsoluteCyndricalCSYS(NXOpen.CAE.Post.AxisymetricAxis.None)

    resultParameters1.SetBeamResultsInLocalCoordinateSystem(True)

    resultParameters1.MakeElementResult(False)

    resultParameters1.SetElementValueCriterion(NXOpen.CAE.Result.ElementValueCriterion.Average)

    Dim average1 As NXOpen.CAE.Result.Averaging = Nothing
    average1.DoAveraging = False
    average1.AverageAcrossPropertyIds = True
    average1.AverageAcrossMaterialIds = True
    average1.AverageAcrossElementTypes = True
    average1.AverageAcrossFeatangle = True
    average1.AverageAcrossAnglevalue = 45.0
    average1.IncludeInternalElementContributions = True
    resultParameters1.SetAveragingCriteria(average1)

    resultParameters1.SetComputationType(NXOpen.CAE.Result.ComputationType.None)

    resultParameters1.SetExcludeElementsNotVisible(False)

    resultParameters1.SetComplexCriterion(NXOpen.CAE.Result.Complex.Real)

    resultParameters1.SetPhaseAngle(0.0)

    resultParameters1.SetSectionPlyLayer(0, 0, 1)

    resultParameters1.SetScale(1.0)

    Dim unit1 As NXOpen.Unit = CType(workSimPart.UnitCollection.FindObject("MilliMeter"), NXOpen.Unit)

    resultParameters1.SetUnit(unit1)

    resultParameters1.SetAbsoluteValue(False)

    resultParameters1.SetCalculateBeamStrResults(False)

    resultParameters1.SetBeamFillets(False)

    resultParameters1.SetBeamFilletRadius(0.5)

    resultParameters1.DisplayMidnodeValue(True)

    resultParameters1.SetIsReferenceNode(False)

    resultParameters1.SetReferenceNodeLabel(0)

    Dim cyclicSymmetricParameters1 As NXOpen.CAE.CyclicSymmetricParameters = Nothing
    cyclicSymmetricParameters1 = resultParameters1.GetCyclicSymmetricParameters()

    cyclicSymmetricParameters1.ResultOption = NXOpen.CAE.CyclicSymmetricParameters.GetResult.OnOriginalModel

    cyclicSymmetricParameters1.OriginalResultOption = NXOpen.CAE.CyclicSymmetricParameters.OriginalResult.BySector

    cyclicSymmetricParameters1.SectCriteria = NXOpen.CAE.CyclicSymmetricParameters.SectorCriteria.Index

    cyclicSymmetricParameters1.SectorValue = NXOpen.CAE.CyclicSymmetricParameters.Value.Maximum

    cyclicSymmetricParameters1.EnvValue = NXOpen.CAE.CyclicSymmetricParameters.EnvelopeValue.Average

    cyclicSymmetricParameters1.SectorIndex = 1

    Dim sectors1(-1) As Integer
    cyclicSymmetricParameters1.SetSectorIndices(sectors1)

    Dim axiSymmetricParameters1 As NXOpen.CAE.AxiSymmetricParameters = Nothing
    axiSymmetricParameters1 = resultParameters1.GetAxiSymmetricParameters()

    axiSymmetricParameters1.ResultOption = NXOpen.CAE.AxiSymmetricParameters.GetResult.OnOriginalModel

    axiSymmetricParameters1.RotationAxis = NXOpen.CAE.AxiSymmetricParameters.AxisOfRotation.XAxis

    axiSymmetricParameters1.AxiOptions = NXOpen.CAE.AxiSymmetricParameters.Options.AtRevolveAngle

    axiSymmetricParameters1.EnvelopeVal = NXOpen.CAE.AxiSymmetricParameters.EnvVal.Average

    axiSymmetricParameters1.RevolveAngle = 0.0

    axiSymmetricParameters1.StartRevolveAngle = 0.0

    axiSymmetricParameters1.EndRevolveAngle = 360.0

    axiSymmetricParameters1.NumberOfSections = 40

    theSession.Post.PostviewSetResult(3, resultParameters1)

    Dim deformationParameters1 As NXOpen.CAE.DeformationParameters = Nothing
    deformationParameters1 = theSession.ResultManager.CreateDeformationParameters()

    deformationParameters1.GenericType = resultType1

    deformationParameters1.ComplexCriterion = NXOpen.CAE.Result.Complex.Real

    deformationParameters1.PhaseAngle = 0.0

    deformationParameters1.DeformationType = NXOpen.CAE.Result.DeformationScale.Model

    'Apply deformations scale (in percent)
    deformationParameters1.Scale = 2.0

    deformationParameters1.Component = NXOpen.CAE.Result.Component.Magnitude

    deformationParameters1.IsReferenceNode = False

    deformationParameters1.ReferenceNodeLabel = 0

    deformationParameters1.InitialDeformationScaleType = -1

    deformationParameters1.InitialDeformationScale = 1.0

    deformationParameters1.InitialDeformationUserselectionType = NXOpen.CAE.Result.InitialDeformationSelection.None

    theSession.Post.PostviewSetDeformation(3, deformationParameters1)

    theSession.ResultManager.DeleteDeformationParameters(deformationParameters1)

    theSession.Post.PostviewUpdate(3)

    'Define image output
    Dim jpeg_output As String = "C:\temp\test" & cstr(i) & ".jpg"
    Dim ufs As UFSession = UFSession.GetUFSession()
    ufs.Disp.CreateImage(jpeg_output, UFDisp.ImageFormat.Jpeg, UFDisp.BackgroundColor.White)

Next i

'Kill Result parameters
theSession.ResultManager.DeleteResultParameters(resultParameters1)

End Sub
End Module
