REM UN-Register COM Server for Prelude Chatbot:
regasm /u PreludeCOM.dll

REM UN-Register utility dll:
regasm /u st.dll

REM UN-Install COM Server in global assembly cache:
gacutil /u PreludeCOM.dll

REM do the same for the utility
gacutil /u st.dll