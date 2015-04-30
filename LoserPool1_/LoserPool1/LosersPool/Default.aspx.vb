Imports System
Imports System.Data
Imports System.Linq
Imports System.Xml.Linq
Imports System.Globalization
Imports System.Threading

Imports LoserPool1.JoinPools.Models
Imports LoserPool1.LosersPool.Models
'Imports LoserPool1.LosersPool.Administration


Public Class _Default2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("userId") Is Nothing Then
            Response.Redirect("~/Account/Login.aspx")
        End If

        'First two are need for to use test driver
        'read UserList XML and save to empty Users Table of LoserPool2 database
        Dim userList1 = New UserList("C:\Users\Larry\Documents\GitHub\Russbucks-Test\LoserPool1_\LoserPool1\TestFiles\UserList.xml")

        ' read userChoices XML and save to empty userChoices Table of LoserPool2 database
        Dim userChoices1 = New UserChoiceList("C:\Users\Larry\Documents\GitHub\Russbucks-Test\LoserPool1_\LoserPool1\TestFiles\UserChoicesList.xml", "week1")

        'If reading only one week of schedule
        'to read a weekly XML schedule file uncomment the following statement
        'Dim schedule = New ReadScheduleFile("onefile")

        'to read all weekly XML schedule files
        Dim schedule1 = New ReadScheduleFile("manyfiles")

        ' query the schedule table in LoserPool2 to add minstart times and dates to the ScheduleTimePeriod Database
        Dim scheduleTimePeriod1 = New CreateSchedulePeriod

        'Need if reading scheduleTimePeriods from File (unlikely)
        ' read the schedultTimePeriod XML file and save to ScheduleTimePeriod  Table of LoserPool2 Database
        'Dim scheduleTimePeriod2 = New ReadScheduleTimePeriodXMLFile("C:\Users\Larry\Documents\GitHub\Russbucks-Test\LoserPool1_\LoserPool1\TestFiles\scheduleTimePeriod.xml")

        Dim dummy = "dummy"
    End Sub

    Public Function GetMyOptions() As IQueryable(Of User)

        Dim EName As String = CStr(Session("userId"))

        Dim mlpo As New MyOptionsList(EName)

        Session("weekNumber") = mlpo.weekNumber

        Dim query1

        Dim _dBLoserPool As New LosersPoolContext

        Try
            'needs to be set to number of weeks in the season + 1

            If mlpo.weekNumber = "week4" Then
                Response.Redirect("~/LosersPool/Season End.aspx")
            End If


            query1 = From user1 In _dBLoserPool.Users
                       Where user1.UserId = EName
                       Select user1

            For Each user1 In query1

                Session("optionState") = user1.OptionState

                If user1.OptionState = "Scoring Update" Then
                    enterUserData.Visible = False
                ElseIf user1.OptionState = "Enter Picks" Then
                    scoringUpdate.Visible = False
                ElseIf user1.OptionState = "SeasonEnd" Then
                    Response.Redirect("~/LosersPool/Season End.aspx")
                ElseIf user1.optionState = "ScoringUpdateNotReady" Then
                    Response.Redirect("~/LosersPool/UpdateNotReady.aspx")
                End If

            Next

            _dBLoserPool.SaveChanges()

            Return query1


        Catch ex As Exception

        End Try

        Return Nothing

    End Function

End Class

Public Class MyOptionsList
    Public Property optionsList As MyPool
    Public Property weekNumber As String

    Public Sub New(Ename)
        Me.optionsList = MyOptionsList1(Me, Ename)
    End Sub

    Public Shared Function MyOptionsList1(mlpo As MyOptionsList, Ename As String) As MyPool

        Dim myPool1 As New MyPool

        Dim fileDirectory = "C:\Users\Larry\Documents\GitHub\Russbucks-Test\LoserPool1_\LoserPool1\TestFiles\"
        Dim finalScoringUpdateFile = "scoringUpdateFinalWeek"

        'Dim GameUpdateCollection As New Dictionary(Of String, GameUpdate)

        Dim _dbLoserPool As New LosersPoolContext

        Try
            Using (_dbLoserPool)

                Dim currentDateTime As DateTime = Date.Now

                Dim myUpdate = XDocument.Load("C:\Users\Larry\Documents\GitHub\Russbucks-Test\LoserPool1_\LoserPool1\TestFiles\scoringUpdate.xml")

                ' Get update file time and date

                Dim queryTime = (From score In myUpdate.Descendants("scores")
                                 Select New ScoreUpdateXML With {.filetime = score.Attribute("filetime").Value,
                                                                 .filedate = score.Attribute("filedate").Value,
                                                                 .weeknumber = score.Attribute("weeknumber").Value}).ToList

                ' Make sure user option state is initialized

                If queryTime(0).weeknumber = "week0" Then
                    Dim queryUser = (From user1 In _dbLoserPool.Users
                                    Where user1.UserName = Ename).ToList
                    For Each user1 In queryUser
                        user1.OptionState = "Enter Picks"
                    Next
                    _dbLoserPool.SaveChanges()
                End If


                Dim fileTime = DateTime.Parse(CDate(queryTime(0).filedate) + " " + CDate(queryTime(0).filetime))
                Dim fileWeekNumber = queryTime(0).weeknumber


                '  Get games in update file

                Dim queryGame = (From game In myUpdate.Descendants("scores").Descendants("game")
                                 Select New GameUpdateXML With {.hometeam = game.Attribute("hometeam").Value,
                                                               .homescore = CInt(game.Elements("homescore").Value),
                                                               .awayteam = game.Attribute("awayteam").Value,
                                                               .awayscore = CInt(game.Elements("awayscore").Value),
                                                               .gametime = game.Elements("gametime").Value}).ToList


                ' get  games from schedule which are not final
                Dim queryGameSchedule As New List(Of ScheduleEntity)
                queryGameSchedule = (From game1 In _dbLoserPool.ScheduleEntities
                                     Where game1.GameTime <> "final").ToList


                ' find earlist week number where games are not final

                Dim minWeekNumber = 3
                For Each game1 In queryGameSchedule

                    Dim weekNumber = CInt(Mid(game1.WeekId, 5))

                    If weekNumber < minWeekNumber Then
                        minWeekNumber = weekNumber
                    End If

                Next

                ' Find which week we are in

                Dim thisWeek = "week" + CStr(minWeekNumber)
                Dim lastWeek = "week" + CStr(minWeekNumber - 1)

                Dim queryTimePeriod As New List(Of ScheduleTimePeriod)
                queryTimePeriod = (From timePeriod In _dbLoserPool.ScheduleTimePeriods
                                   Where timePeriod.weekID = lastWeek).ToList

                If lastWeek <> "week0" Then
                    If queryTimePeriod(0).GamesFinishedDate Is Nothing Or queryTimePeriod(0).GamesFinishedTime Is Nothing Then
                        thisWeek = lastWeek
                    End If
                End If

                Dim fWN = CInt(fileWeekNumber.Substring(4))
                Dim tW = CInt(thisWeek.Substring(4))

                Dim scoringUpdateFinalFile = fileDirectory + finalScoringUpdateFile + CStr(fWN) + ".xml"
                Dim sUFF = XDocument.Load(scoringUpdateFinalFile)
                Dim queryFinalTime = (From score In sUFF.Descendants("scores")
                             Select New ScoreUpdateXML With {.filetime = score.Attribute("filetime").Value,
                                                             .filedate = score.Attribute("filedate").Value,
                                                             .weeknumber = score.Attribute("weeknumber").Value}).ToList

                Dim fileFinalTime = DateTime.Parse(CDate(queryFinalTime(0).filedate) + " " + CDate(queryFinalTime(0).filetime))
                Dim fileFinalWeekNumber = queryFinalTime(0).weeknumber

                Dim WeekIsFinished = False
                If fileTime = fileFinalTime And fileWeekNumber = fileFinalWeekNumber Then
                    WeekIsFinished = True
                End If

                Dim finishedWeeks = fWN - 1
                If WeekIsFinished = True Then
                    finishedWeeks = fWN
                End If

                ' Databases needs to be updated due to inactivity
                If fWN > tW Then
                    For I = tW To finishedWeeks
                        Dim scoringUpdateFile = fileDirectory + finalScoringUpdateFile + CStr(I) + ".xml"
                        Dim sUF = XDocument.Load(scoringUpdateFile)

                        queryTime = (From score In sUF.Descendants("scores")
                                         Select New ScoreUpdateXML With {.filetime = score.Attribute("filetime").Value,
                                                                         .filedate = score.Attribute("filedate").Value,
                                                                         .weeknumber = score.Attribute("weeknumber").Value}).ToList

                        queryGame = (From game In sUF.Descendants("scores").Descendants("game")
                                         Select New GameUpdateXML With {.hometeam = game.Attribute("hometeam").Value,
                                                                       .homescore = CInt(game.Elements("homescore").Value),
                                                                       .awayteam = game.Attribute("awayteam").Value,
                                                                       .awayscore = CInt(game.Elements("awayscore").Value),
                                                                       .gametime = game.Elements("gametime").Value}).ToList

                        Dim I1 = I
                        queryTimePeriod = (From weekNum In _dbLoserPool.ScheduleTimePeriods
                                            Where weekNum.weekID = "week" + CStr(I1) Or weekNum.weekID = "week" + CStr(I1 + 1)
                                            Order By weekNum.weekID).ToList

                        queryTimePeriod(0).GamesFinishedDate = queryTime(0).filedate
                        queryTimePeriod(0).GamesFinishedTime = queryTime(0).filetime

                        If queryTimePeriod.Count > 1 Then
                            queryTimePeriod(1).UserEntryStartDate = queryTime(0).filedate
                            queryTimePeriod(1).UserEntryStartTime = queryTime(0).filetime
                        End If

                        _dbLoserPool.SaveChanges()

                        mlpo.weekNumber = "week" + CStr(I + 1)

                        queryGameSchedule = (From game1 In _dbLoserPool.ScheduleEntities
                                     Where game1.WeekId = "week" + CStr(I1)).ToList

                        For Each game1 In queryGameSchedule

                            Dim queryUpdateGame1 = (From game In sUF.Descendants("scores").Descendants("game")
                                                    Where game.Attribute("hometeam").Value = game1.HomeTeam Or game.Attribute("awayteam").Value = game1.AwayTeam)

                            If queryUpdateGame1.Count = 0 Then
                                Continue For
                            End If

                            Dim queryUpdateGame As New List(Of GameUpdateXML)

                            queryUpdateGame = (From game In sUF.Descendants("scores").Descendants("game")
                                                  Where game.Attribute("hometeam").Value = game1.HomeTeam And game.Attribute("awayteam").Value = game1.AwayTeam
                                                  Select New GameUpdateXML With {.hometeam = game.Attribute("hometeam").Value,
                                                                           .homescore = game.Elements("homescore").Value,
                                                                           .awayteam = game.Attribute("awayteam").Value,
                                                                           .awayscore = game.Elements("awayscore").Value,
                                                                           .gametime = game.Elements("gametime").Value}).ToList

                            game1.HomeScore = queryUpdateGame(0).homescore
                            game1.AwayScore = queryUpdateGame(0).awayscore
                            game1.GameTime = queryUpdateGame(0).gametime

                            _dbLoserPool.SaveChanges()
                        Next

                        Dim myUser1 As New User
                        myUser1 = _dbLoserPool.Users.SingleOrDefault(Function(mU) mU.UserId = Ename)
                        myUser1.OptionState = "Enter Picks"

                        ' Get all contenders

                        Dim queryUserChoices1 = (From user2 In _dbLoserPool.UserChoicesList
                                                Where user2.WeekId = "week" + CStr(I1) And user2.Contender = True
                                                Select user2).ToList

                        For Each user1 In queryUserChoices1

                            ' Make sure contender is not  already on the loser list

                            Dim queryLoser = (From loser1 In _dbLoserPool.LoserList
                                              Where loser1.UserName = user1.UserName
                                              Select loser1).ToList

                            If queryLoser.Count = 0 Then

                                If user1.UserPick Is Nothing Or user1.UserPick = "" Then
                                    ' user1 is a loser
                                    user1.Contender = False

                                    Dim loser1 = New Loser
                                    loser1.ListId = _dbLoserPool.LoserList.Count + 1
                                    loser1.UserId = user1.UserID
                                    loser1.UserName = user1.UserName
                                    loser1.WeekId = user1.WeekId
                                    loser1.WeekIdInt = CInt(Mid(user1.WeekId, 5))
                                    loser1.LosingPick = "Not Made"
                                    _dbLoserPool.LoserList.Add(loser1)
                                    _dbLoserPool.SaveChanges()
                                    Continue For

                                End If
                            End If

                            ' Finalize scores in schedule and determine if user is a contender or a loser

                            For Each game In queryGame
                                If game.hometeam = user1.UserPick Or game.awayteam = user1.UserPick Then
                                    If game.hometeam = user1.UserPick Then
                                        If game.homescore < game.awayscore Then
                                            'user1 is still a contender
                                            ' set user1 pick team to false
                                            Dim user2 = New UserChoices

                                            user2.ListId = _dbLoserPool.UserChoicesList.Count + 1
                                            user2.UserID = user1.UserID
                                            user2.UserName = user1.UserName
                                            user2.WeekId = "week" + CStr(CInt(Mid(thisWeek, 5)) + 1)  'weekId
                                            user2.SeaHawks = user1.SeaHawks
                                            user2.Packers = user1.Packers
                                            user2.Falcons = user1.Falcons
                                            user2.Saints = user1.Saints
                                            user2.Chargers = user1.Chargers
                                            user2.Cardinals = user1.Cardinals
                                            user2.Lions = user1.Lions
                                            user2.Giants = user1.Giants
                                            user2.Contender = True
                                            user2.UserPick = user1.UserPick
                                            user2 = SetContendersPickToFalse(user2)
                                            user2.UserPick = ""
                                            _dbLoserPool.UserChoicesList.Add(user2)

                                            _dbLoserPool.SaveChanges()
                                            Exit For
                                        Else
                                            'user1 is a loser
                                            'set user1 contender to false
                                            user1.Contender = False
                                            'add  user1 to loser list
                                            Dim loser1 = New Loser
                                            loser1.ListId = _dbLoserPool.LoserList.Count + 1
                                            loser1.UserId = user1.UserID
                                            loser1.UserName = user1.UserName
                                            loser1.WeekId = user1.WeekId
                                            loser1.WeekIdInt = CInt(Mid(user1.WeekId, 5))
                                            loser1.LosingPick = user1.UserPick
                                            _dbLoserPool.LoserList.Add(loser1)
                                            _dbLoserPool.SaveChanges()
                                            Exit For
                                        End If
                                    Else
                                        If game.awayscore < game.homescore Then
                                            'user1 is still a contender
                                            ' set user1 pick team to false

                                            Dim user2 = New UserChoices
                                            user2.ListId = _dbLoserPool.UserChoicesList.Count + 1
                                            user2.UserID = user1.UserID
                                            user2.UserName = user1.UserName
                                            user2.WeekId = "week" + CStr(CInt(Mid(thisWeek, 5)) + 1)  'weekId
                                            user2.SeaHawks = user1.SeaHawks
                                            user2.Packers = user1.Packers
                                            user2.Falcons = user1.Falcons
                                            user2.Saints = user1.Saints
                                            user2.Chargers = user1.Chargers
                                            user2.Cardinals = user1.Cardinals
                                            user2.Lions = user1.Lions
                                            user2.Giants = user1.Giants
                                            user2.Contender = True
                                            user2.UserPick = user1.UserPick
                                            user2 = SetContendersPickToFalse(user2)
                                            user2.UserPick = ""
                                            _dbLoserPool.UserChoicesList.Add(user2)

                                            _dbLoserPool.SaveChanges()
                                            Exit For
                                        Else
                                            'user1 is a loser
                                            'set user1 contender to false
                                            user1.Contender = False
                                            'add  user1 to loser list
                                            Dim loser1 = New Loser
                                            loser1.ListId = _dbLoserPool.LoserList.Count + 1
                                            loser1.UserId = user1.UserID
                                            loser1.UserName = user1.UserName
                                            loser1.WeekId = user1.WeekId
                                            loser1.WeekIdInt = CInt(Mid(user1.WeekId, 5))
                                            loser1.LosingPick = user1.UserPick
                                            _dbLoserPool.LoserList.Add(loser1)
                                            _dbLoserPool.SaveChanges()
                                            Exit For
                                        End If

                                    End If
                                End If
                            Next
                        Next

                        thisWeek = "week" + CStr(I + 1)
                        _dbLoserPool.SaveChanges()

                    Next
                End If

                ' Update scores for  current games on the schedule

                queryGameSchedule = New List(Of ScheduleEntity)
                queryGameSchedule = (From game1 In _dbLoserPool.ScheduleEntities
                                     Where game1.WeekId = thisWeek).ToList

                If queryTime(0).weeknumber <> "week0" Then
                    For Each game1 In queryGameSchedule

                        Dim queryUpdateGame1 = (From game In myUpdate.Descendants("scores").Descendants("game")
                                                Where game.Attribute("hometeam").Value = game1.HomeTeam Or game.Attribute("awayteam").Value = game1.AwayTeam)

                        If queryUpdateGame1.Count = 0 Then
                            Continue For
                        End If

                        Dim queryUpdateGame As New List(Of GameUpdateXML)

                        queryUpdateGame = (From game In myUpdate.Descendants("scores").Descendants("game")
                                              Where game.Attribute("hometeam").Value = game1.HomeTeam And game.Attribute("awayteam").Value = game1.AwayTeam
                                              Select New GameUpdateXML With {.hometeam = game.Attribute("hometeam").Value,
                                                                       .homescore = game.Elements("homescore").Value,
                                                                       .awayteam = game.Attribute("awayteam").Value,
                                                                       .awayscore = game.Elements("awayscore").Value,
                                                                       .gametime = game.Elements("gametime").Value}).ToList

                        game1.HomeScore = queryUpdateGame(0).homescore
                        game1.AwayScore = queryUpdateGame(0).awayscore
                        game1.GameTime = queryUpdateGame(0).gametime


                        _dbLoserPool.SaveChanges()

                    Next

                End If

                'Determine if games are finished from updated Schedule

                WeekIsFinished = True
                For Each game In queryGameSchedule
                    If Not (game.GameTime = "final") Then
                        WeekIsFinished = False
                        Exit For
                    End If
                Next

                ' Update all necessary lists and route user by updating the options state
                ' The user can being in the "Enter Picks" state, the "Scoring Update" state or the "SeasonEnd" state
                'The lists updated are The ScheduleTimePeriods list,

                queryTimePeriod = (From timeperiod1 In _dbLoserPool.ScheduleTimePeriods
                                  Where timeperiod1.weekID = thisWeek).ToList

                If WeekIsFinished = True Then

                    For Each timePeriod In queryTimePeriod

                        ' Convert dates and times to datetimes

                        Dim startWeekTime As DateTime?
                        startWeekTime = Nothing
                        If Not (timePeriod.UserEntryStartDate Is Nothing) And Not (timePeriod.UserEntryStartTime Is Nothing) Then
                            startWeekTime = DateTime.Parse(timePeriod.UserEntryStartDate + " " + timePeriod.UserEntryStartTime)
                        End If

                        Dim startGameTime As DateTime?
                        startGameTime = Nothing
                        If Not (timePeriod.UserEntryFinshedGamesStartDate Is Nothing) And Not (timePeriod.UserEntryFinshedGamesStartTime Is Nothing) Then
                            startGameTime = DateTime.Parse(timePeriod.UserEntryFinshedGamesStartDate + " " + timePeriod.UserEntryFinshedGamesStartTime)
                        End If

                        Dim endWeekTime As DateTime?
                        endWeekTime = Nothing
                        If Not (timePeriod.GamesFinishedDate Is Nothing) And Not (timePeriod.GamesFinishedTime Is Nothing) Then
                            endWeekTime = DateTime.Parse(timePeriod.GamesFinishedDate + " " + timePeriod.GamesFinishedTime)
                        End If

                        ' Find the time period which the user is in

                        If (startGameTime < currentDateTime) And (endWeekTime Is Nothing) And fileTime >= startGameTime Then

                            'all games in scoring update file are final but Schedule Update Database needs to be updated
                            'user can enter picks for new week
                            ' my loser pool options (mlpo) list week is updated
                            ' users table is updated
                            ' losers table is updated
                            ' user choices table is updated

                            timePeriod.GamesFinishedDate = queryTime(0).filedate
                            timePeriod.GamesFinishedTime = queryTime(0).filetime

                            mlpo.weekNumber = "week" + CStr(CInt(Mid(thisWeek, 5)) + 1)

                            'All games are final Schedule Time Periods table will be updated

                            Dim queryTimePeriod1 As New ScheduleTimePeriod

                            queryTimePeriod1 = _dbLoserPool.ScheduleTimePeriods.SingleOrDefault(Function(qTP) qTP.weekID = mlpo.weekNumber)
                            If queryTimePeriod1 Is Nothing Then
                            Else
                                queryTimePeriod1.UserEntryStartDate = queryTime(0).filedate
                                queryTimePeriod1.UserEntryStartTime = queryTime(0).filetime
                            End If

                            Dim myUser1 As New User
                            myUser1 = _dbLoserPool.Users.SingleOrDefault(Function(mU) mU.UserId = Ename)
                            myUser1.OptionState = "Enter Picks"

                            ' Get all contenders

                            Dim queryUserChoices1 = (From user2 In _dbLoserPool.UserChoicesList
                                                    Where user2.WeekId = timePeriod.weekID And user2.Contender = True
                                                    Select user2).ToList

                            For Each user1 In queryUserChoices1

                                ' Make sure contender is not  already on the loser list

                                Dim queryLoser = (From loser1 In _dbLoserPool.LoserList
                                                  Where loser1.UserName = user1.UserName
                                                  Select loser1).ToList

                                If queryLoser.Count = 0 Then

                                    If user1.UserPick Is Nothing Or user1.UserPick = "" Then
                                        ' user1 is a loser
                                        user1.Contender = False

                                        Dim loser1 = New Loser
                                        loser1.ListId = _dbLoserPool.LoserList.Count + 1
                                        loser1.UserId = user1.UserID
                                        loser1.UserName = user1.UserName
                                        loser1.WeekId = user1.WeekId
                                        loser1.WeekIdInt = CInt(Mid(user1.WeekId, 5))
                                        loser1.LosingPick = user1.UserPick
                                        _dbLoserPool.LoserList.Add(loser1)
                                        _dbLoserPool.SaveChanges()
                                        Continue For

                                    End If
                                End If

                                ' Finalize scores in schedule and determine if user is a contender or a loser

                                For Each game In queryGame
                                    If game.hometeam = user1.UserPick Or game.awayteam = user1.UserPick Then
                                        If game.hometeam = user1.UserPick Then
                                            If game.homescore < game.awayscore Then
                                                'user1 is still a contender
                                                ' set user1 pick team to false
                                                Dim user2 = New UserChoices

                                                user2.ListId = _dbLoserPool.UserChoicesList.Count + 1
                                                user2.UserID = user1.UserID
                                                user2.UserName = user1.UserName
                                                user2.WeekId = "week" + CStr(CInt(Mid(thisWeek, 5)) + 1)  'weekId
                                                user2.SeaHawks = user1.SeaHawks
                                                user2.Packers = user1.Packers
                                                user2.Falcons = user1.Falcons
                                                user2.Saints = user1.Saints
                                                user2.Chargers = user1.Chargers
                                                user2.Cardinals = user1.Cardinals
                                                user2.Lions = user1.Lions
                                                user2.Giants = user1.Giants
                                                user2.Contender = True
                                                user2.UserPick = user1.UserPick
                                                user2 = SetContendersPickToFalse(user2)
                                                user2.UserPick = ""
                                                _dbLoserPool.UserChoicesList.Add(user2)

                                                _dbLoserPool.SaveChanges()
                                                Exit For
                                            Else
                                                'user1 is a loser
                                                'set user1 contender to false
                                                user1.Contender = False
                                                'add  user1 to loser list
                                                Dim loser1 = New Loser
                                                loser1.ListId = _dbLoserPool.LoserList.Count + 1
                                                loser1.UserId = user1.UserID
                                                loser1.UserName = user1.UserName
                                                loser1.WeekId = user1.WeekId
                                                loser1.WeekIdInt = CInt(Mid(user1.WeekId, 5))
                                                loser1.LosingPick = user1.UserPick
                                                _dbLoserPool.LoserList.Add(loser1)
                                                _dbLoserPool.SaveChanges()
                                                Exit For
                                            End If
                                        Else
                                            If game.awayscore < game.homescore Then
                                                'user1 is still a contender
                                                ' set user1 pick team to false

                                                Dim user2 = New UserChoices
                                                user2.ListId = _dbLoserPool.UserChoicesList.Count + 1
                                                user2.UserID = user1.UserID
                                                user2.UserName = user1.UserName
                                                user2.WeekId = "week" + CStr(CInt(Mid(thisWeek, 5)) + 1)  'weekId
                                                user2.SeaHawks = user1.SeaHawks
                                                user2.Packers = user1.Packers
                                                user2.Falcons = user1.Falcons
                                                user2.Saints = user1.Saints
                                                user2.Chargers = user1.Chargers
                                                user2.Cardinals = user1.Cardinals
                                                user2.Lions = user1.Lions
                                                user2.Giants = user1.Giants
                                                user2.Contender = True
                                                user2.UserPick = user1.UserPick
                                                user2 = SetContendersPickToFalse(user2)
                                                user2.UserPick = ""
                                                _dbLoserPool.UserChoicesList.Add(user2)

                                                _dbLoserPool.SaveChanges()
                                                Exit For
                                            Else
                                                'user1 is a loser
                                                'set user1 contender to false
                                                user1.Contender = False
                                                'add  user1 to loser list
                                                Dim loser1 = New Loser
                                                loser1.ListId = _dbLoserPool.LoserList.Count + 1
                                                loser1.UserId = user1.UserID
                                                loser1.UserName = user1.UserName
                                                loser1.WeekId = user1.WeekId
                                                loser1.WeekIdInt = CInt(Mid(user1.WeekId, 5))
                                                loser1.LosingPick = user1.UserPick
                                                _dbLoserPool.LoserList.Add(loser1)
                                                _dbLoserPool.SaveChanges()
                                                Exit For
                                            End If

                                        End If
                                    End If
                                Next
                            Next

                            _dbLoserPool.SaveChanges()

                            ' myPool1 is updated
                            myPool1.EName = Ename
                            myPool1.LoserPoolOptions.Add("Enter Picks")
                            Return myPool1

                        ElseIf startGameTime < currentDateTime And endWeekTime > currentDateTime And Not (endWeekTime Is Nothing) Then

                            ' Scoring Update File is current but Schedule Period database was already updated - user is looked out of user entry but can see scoring updates
                            ' users table is updated
                            ' myPool1 is updated

                            Dim myUser1 As New User
                            myUser1 = _dbLoserPool.Users.SingleOrDefault(Function(mU) mU.UserId = Ename)
                            myUser1.OptionState = "Scoring Update"
                            _dbLoserPool.SaveChanges()

                            mlpo.weekNumber = timePeriod.weekID
                            myPool1.EName = Ename
                            myPool1.LoserPoolOptions.Add("Scoring Update")
                            Return myPool1

                        ElseIf startWeekTime < currentDateTime And startGameTime > currentDateTime And startWeekTime >= fileTime Then

                            'Scoring Updates file is old file  and will not be used to update Schedule Period Database but user is in data entry time period
                            ' users table is updated
                            ' myPool1 is updated

                            Dim myUser1 As New User
                            myUser1 = _dbLoserPool.Users.SingleOrDefault(Function(mU) mU.UserId = Ename)
                            myUser1.OptionState = "Enter Picks"
                            _dbLoserPool.SaveChanges()

                            mlpo.weekNumber = timePeriod.weekID
                            myPool1.EName = Ename
                            myPool1.LoserPoolOptions.Add("Enter Picks")
                            Return myPool1
                        End If
                    Next

                    ' Not all the games on the schedule are finished

                ElseIf WeekIsFinished = False Then

                    For Each timePeriod In queryTimePeriod

                        Dim startWeekTime As DateTime?
                        startWeekTime = Nothing
                        If Not (timePeriod.UserEntryStartDate Is Nothing) And Not (timePeriod.UserEntryStartTime Is Nothing) Then
                            startWeekTime = DateTime.Parse(timePeriod.UserEntryStartDate + " " + timePeriod.UserEntryStartTime)
                        End If

                        Dim startGameTime As DateTime?
                        startGameTime = Nothing
                        If Not (timePeriod.UserEntryFinshedGamesStartDate Is Nothing) And Not (timePeriod.UserEntryFinshedGamesStartTime Is Nothing) Then
                            startGameTime = DateTime.Parse(timePeriod.UserEntryFinshedGamesStartDate + " " + timePeriod.UserEntryFinshedGamesStartTime)
                        End If

                        Dim endWeekTime As DateTime?
                        endWeekTime = Nothing
                        If Not (timePeriod.GamesFinishedDate Is Nothing) And Not (timePeriod.GamesFinishedTime Is Nothing) Then
                            endWeekTime = DateTime.Parse(timePeriod.GamesFinishedDate + " " + timePeriod.GamesFinishedTime)
                        End If

                        Dim queryUserChoices1 = (From user2 In _dbLoserPool.UserChoicesList
                                                Where user2.WeekId = timePeriod.weekID And user2.Contender = True
                                                Select user2).ToList

                        For Each user1 In queryUserChoices1
                            If startWeekTime Is Nothing Then
                            Else
                                If endWeekTime Is Nothing And currentDateTime > startGameTime And (user1.UserPick Is Nothing Or user1.UserPick = "") Then
                                    'user1 is a loser because user did not enter data
                                    'set user1 contender to false
                                    user1.Contender = False
                                    'add  user1 to loser list

                                    Dim queryLoser = (From loser2 In _dbLoserPool.LoserList
                                                      Where loser2.UserName = user1.UserName
                                                      Select loser2).ToList

                                    If queryLoser.Count = 0 Then
                                        Dim loser1 = New Loser
                                        loser1.ListId = _dbLoserPool.LoserList.Count + 1
                                        loser1.UserId = user1.UserID
                                        loser1.UserName = user1.UserName
                                        loser1.WeekId = user1.WeekId
                                        loser1.WeekIdInt = CInt(Mid(user1.WeekId, 5))
                                        loser1.LosingPick = "Not Made"
                                        _dbLoserPool.LoserList.Add(loser1)
                                        _dbLoserPool.SaveChanges()
                                        Continue For
                                    End If
                                End If
                            End If

                        Next

                        ' Route contenders by forming myoptions list

                        If startWeekTime Is Nothing Then

                        Else
                            If endWeekTime Is Nothing And startGameTime <= fileTime Then
                                'Scoring update file is current but Games are not finished user can see scoring updates
                                ' users table is updated
                                ' myPool1 is updated

                                Dim myUser1 As New User
                                myUser1 = _dbLoserPool.Users.SingleOrDefault(Function(mU) mU.UserId = Ename)
                                myUser1.OptionState = "Scoring Update"
                                _dbLoserPool.SaveChanges()

                                mlpo.weekNumber = timePeriod.weekID
                                myPool1.EName = Ename
                                myPool1.LoserPoolOptions.Add("Scoring Update")
                                Return myPool1

                            ElseIf endWeekTime Is Nothing And (startWeekTime < currentDateTime And startGameTime > currentDateTime) Then
                                'Games for week haven't started
                                ' users table is updated
                                ' myPool1 is updated

                                Dim myUser1 As New User
                                myUser1 = _dbLoserPool.Users.SingleOrDefault(Function(mU) mU.UserId = Ename)
                                myUser1.OptionState = "Enter Picks"
                                _dbLoserPool.SaveChanges()

                                mlpo.weekNumber = timePeriod.weekID
                                myPool1.EName = Ename
                                myPool1.LoserPoolOptions.Add("Enter Picks")
                                Return myPool1

                            ElseIf endWeekTime Is Nothing And startGameTime < currentDateTime And fileTime < startGameTime Then
                                'Games have started but Scoring Update file is not current
                                'users table is updated
                                'mypool1 is updated

                                Dim myUser1 As New User
                                myUser1 = _dbLoserPool.Users.SingleOrDefault(Function(mU) mU.UserId = Ename)
                                myUser1.OptionState = "ScoringUpdateNotReady"
                                _dbLoserPool.SaveChanges()

                                mlpo.weekNumber = timePeriod.weekID
                                myPool1.EName = Ename
                                myPool1.LoserPoolOptions.Add("Scoring Update")
                                Return myPool1


                            End If
                        End If
                    Next
                End If
            End Using
        Catch ex As Exception

        End Try

        'All Games Have Played

        'Kitichen sink return don't let user do anything Maybe should return error


        'myPool1.LoserPoolOptions.Add("dummy")
        'myPool1.LoserPoolOptions.Add("dummy")

        Return myPool1

    End Function

    Private Shared Function SetContendersPickToFalse(user1 As UserChoices) As UserChoices
        If user1.UserPick = "seahawks" Then
            user1.SeaHawks = False
        ElseIf user1.UserPick = "packers" Then
            user1.Packers = False
        ElseIf user1.UserPick = "falcons" Then
            user1.Falcons = False
        ElseIf user1.UserPick = "saints" Then
            user1.Saints = False
        ElseIf user1.UserPick = "chargers" Then
            user1.Chargers = False
        ElseIf user1.UserPick = "cardinals" Then
            user1.Cardinals = False
        ElseIf user1.UserPick = "lions" Then
            user1.Lions = False
        ElseIf user1.UserPick = "giants" Then
            user1.Giants = False
        End If
        Return user1
    End Function


End Class