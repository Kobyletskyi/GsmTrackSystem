/***************************************************************************
 *
 *            SIMCOM Application Foreground Layer
 *
 *              Copyright (c) 2012 SIMCOM Ltd.
 *
 ***************************************************************************
 *
 *   $Revision: #1 $
 *   $DateTime: 2012/02/08 09:00:00 $
 *
 ***************************************************************************
 *
 *  Designed by    :   MXN
 *  Coded by       :   MXN
 *  Tested by      :
 *
 ***************************************************************************
 *
 * File Description
 * ----------------
 *   DTMF detection
 *   AT+CRWP=1 to enable DTMF detection
 *   AT+CRWP=0 to disable DTMF detection
 ***************************************************************************
 *
 ***************************************************************************/
#include "fl_stdlib.h"
#include "fl_typ.h"
#include "fl_appinit.h"
#include "fl_interface.h"
#include "fl_error.h"
#include "fl_fcm.h"
#include "fl_audio.h"
#include "fl_trace.h"
FlEventBuffer	__align__ flEventBuffer;
FlEventBuffer	__align__ flEventBufferM1;
FlEventBuffer	__align__ flEventBufferM2;
FlEventBuffer	__align__ flEventBufferM3;
FlEventBuffer	__align__ flEventBufferM4;
FlEventBuffer	__align__ flEventBufferM5;
extern void waitForSysInt(void);
extern void fl_MultiTaskPrio1(void);
extern void fl_MultiTaskPrio2(void);
extern void fl_MultiTaskPrio3(void);
extern void fl_MultiTaskPrio4(void);
extern void fl_MultiTaskPrio5(void);
/** Init function */
fl_task_entry fl_init = waitForSysInt;

/** Entry functions */
fl_task_entry fl_entries[] = { fl_MultiTaskPrio1, fl_MultiTaskPrio2, fl_MultiTaskPrio3, fl_MultiTaskPrio4, fl_MultiTaskPrio5, NULL };
/***************************************************************************
*  Function:       fl_entry                                                  
*  Object:         Customer application                        
*  Variable Name     |IN |OUT|GLB|  Utilisation                           
***************************************************************************/
void fl_entry()
{
    bool  keepGoing = TRUE;
    u32 para1,i;
		
    ebdat7_00EnterDebugMode();
    while (keepGoing == TRUE)
    {
        memset((u8*)&flEventBuffer,0x00,sizeof(flEventBuffer));
        eat1_02GetEvent(&flEventBuffer);
       
        switch(flEventBuffer.eventTyp)
        {
            case EVENT_MODEMDATA:
            {
								flEventBuffer.eventData.modemdata_evt.data[flEventBuffer.eventData.modemdata_evt.length] = 0;
								ebdat7_01DebugTrace((const char *)flEventBuffer.eventData.modemdata_evt.data);								
								
								if( MODEM_CRWP == flEventBuffer.eventData.modemdata_evt.type )
                {
										/*get AT+CRWP parameters */
										i = sscanf((const char *)flEventBuffer.eventData.modemdata_evt.data, "AT+CRWP=%d",&para1);
										ebdat7_01DebugTrace("\n\rGet %d para: %d, %d, %d\r\n", i, para1);
										if (para1 > 1)
										{
											ebdat7_01DebugTrace("Parameter error");
										}
										if (ebdat10_06DTMFDetectEnable(para1) == FL_OK)/*enable or disable DTMF detection 1 enable, 0 disable*/
										{
											ebdat7_01DebugTrace("successfully");
										}
										else
										{
											ebdat7_01DebugTrace("fail");
										}
                }

								/*MODEM_CMD  or MODEM_DATA */					
								else 
								{
								    ;
								}
            }
            break;
            case EVENT_DTMF:
            	/*show the charactor of the DTMF*/
							ebdat7_01DebugTrace((const char *)"DTMF: %c", flEventBuffer.eventData.dtmf_evt.dtmfChar);
						break;
            default:
            break;
        }
    }
}
void fl_MultiTaskPrio1(void)
{
	while (TRUE)
	{
		eat1_02GetEvent(&flEventBufferM1);
		ebdat7_01DebugTrace((const char *)"fl_MultiTaskPrio1");
	}
}

void fl_MultiTaskPrio2(void)
{
	while (TRUE)
	{
		eat1_02GetEvent(&flEventBufferM2);
		ebdat7_01DebugTrace((const char *)"fl_MultiTaskPrio2");
	}
}

void fl_MultiTaskPrio3(void)
{
	while (TRUE)
	{
		eat1_02GetEvent(&flEventBufferM3);
		ebdat7_01DebugTrace((const char *)"fl_MultiTaskPrio3");
	}
}

void fl_MultiTaskPrio4(void)
{
	while (TRUE)
	{
		eat1_02GetEvent(&flEventBufferM4);
		ebdat7_01DebugTrace((const char *)"fl_MultiTaskPrio4");
	}
}

void fl_MultiTaskPrio5(void)
{
	while (TRUE)
	{
		ebdat7_01DebugTrace((const char *)"fl_MultiTaskPrio5");
		eat1_02GetEvent(&flEventBufferM5);
	}
}

void waitForSysInt(void)
{
	bool v_uartInit = FALSE;
	bool v_fileSystem = FALSE;
	while((v_uartInit == FALSE) ||
		(v_fileSystem == FALSE))
	{
		eat1_02GetEvent(&flEventBuffer);
		switch(flEventBuffer.eventTyp)
		{
			case EVENT_FLASH_READY:
				v_fileSystem = TRUE;
				break;
			case EVENT_UART_READY:
				v_uartInit = TRUE;
				break;
			default:
				break;
		}
	}

}

