REM Register COM Server for Prelude Chatbot:
regasm PreludeCOM.dll

REM Register utility dll:
regasm st.dll

REM Install COM Server in global assembly cache:
gacutil /i PreludeCOM.dll

REM do the same for the utility
gacutil /i st.dll
