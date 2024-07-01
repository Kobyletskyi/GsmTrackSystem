#######################################
##$%^&%&^%*****&^*&*&^*&^&*^*&^*&^*&%&^
#EmbeddedAT900 APPlication Project
# brucefan 
#2010 -6-22 
#Modify by brucefan 2010-07-13
##$%^&%&^%*****&^*&*&^*&^&*^*&^*&^*&%&^
CORETOOL = C:/ARM/SIM900DevIDE_DTMF_MultiTask_GCC/EmbatSIM/gsmos/tools
#######################################
include ./mak/opt.mak
#######################################
ifeq ($(COMP),RVCT)
include $(CORETOOL)/app_make_inc_s
else
include $(CORETOOL)/app_gcc_make_inc_s
endif
#######################################
include ./mak/srclist.mak
#######################################
ifeq ($(COMP),RVCT)
include $(CORETOOL)/app_make_inc_e
else
include $(CORETOOL)/app_gcc_make_inc_e
endif
#######################################