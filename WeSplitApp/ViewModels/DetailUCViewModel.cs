﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WeSplitApp.Model;
using WeSplitApp.Models;

namespace WeSplitApp.ViewModels
{
    public class DetailUCViewModel : BaseViewModel
    {
        //Properties
        private Journey _detailJourney;
        public Journey DetailJourney { get => _detailJourney; set { _detailJourney = value;

                // get Routes list
                DetailRouteList = new AsyncObservableCollection<Route>();
                foreach (var route in DetailJourney.Routes)
                {
                    DetailRouteList.Add(route);
                }

                // get Members List
                DetailMemberList = new AsyncObservableCollection<Member>();
                foreach (var member in DetailJourney.Members)
                {
                    DetailMemberList.Add(member);
                }

                // get Start date
                StartDate = DetailJourney.Departure.ToString().Split(' ').First();

                // get end date
                EndDate = DetailJourney.Arrival.ToString().Split(' ').First();

                // get cover
                string x = "ÿØÿà\0\u0010JFIF\0\u0001\u0001\u0001\0x\0x\0\0ÿÛ\0C\0\u0003\u0002\u0002\u0003\u0002\u0002\u0003\u0003\u0003\u0003\u0004\u0003\u0003\u0004\u0005\b\u0005\u0005\u0004\u0004\u0005\n\a\a\u0006\b\f\n\f\f\v\n\v\v\r\u000e\u0012\u0010\r\u000e\u0011\u000e\v\v\u0010\u0016\u0010\u0011\u0013\u0014\u0015\u0015\u0015\f\u000f\u0017\u0018\u0016\u0014\u0018\u0012\u0014\u0015\u0014ÿÛ\0C\u0001\u0003\u0004\u0004\u0005\u0004\u0005\t\u0005\u0005\t\u0014\r\v\r\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014ÿÀ\0\u0011\b\0\u007f\0Ï\u0003\u0001\"\0\u0002\u0011\u0001\u0003\u0011\u0001ÿÄ\0\u001f\0\0\u0001\u0005\u0001\u0001\u0001\u0001\u0001\u0001\0\0\0\0\0\0\0\0\u0001\u0002\u0003\u0004\u0005\u0006\a\b\t\n\vÿÄ\0µ\u0010\0\u0002\u0001\u0003\u0003\u0002\u0004\u0003\u0005\u0005\u0004\u0004\0\0\u0001}\u0001\u0002\u0003\0\u0004\u0011\u0005\u0012!1A\u0006\u0013Qa\a\"q\u00142\u0081‘¡\b#B±Á\u0015RÑð$3br‚\t\n\u0016\u0017\u0018\u0019\u001a%&'()*456789:CDEFGHIJSTUVWXYZcdefghijstuvwxyzƒ„…†‡ˆ‰Š’“”•–—˜™š¢£¤¥¦§¨©ª²³´µ¶·¸¹ºÂÃÄÅÆÇÈÉÊÒÓÔÕÖ×ØÙÚáâãäåæçèéêñòóôõö÷øùúÿÄ\0\u001f\u0001\0\u0003\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\0\0\0\0\0\0\u0001\u0002\u0003\u0004\u0005\u0006\a\b\t\n\vÿÄ\0µ\u0011\0\u0002\u0001\u0002\u0004\u0004\u0003\u0004\a\u0005\u0004\u0004\0\u0001\u0002w\0\u0001\u0002\u0003\u0011\u0004\u0005!1\u0006\u0012AQ\aaq\u0013\"2\u0081\b\u0014B‘¡±Á\t#3Rð\u0015brÑ\n\u0016$4á%ñ\u0017\u0018\u0019\u001a&'()*56789:CDEFGHIJSTUVWXYZcdefghijstuvwxyz‚ƒ„…†‡ˆ‰Š’“”•–—˜™š¢£¤¥¦§¨©ª²³´µ¶·¸¹ºÂÃÄÅÆÇÈÉÊÒÓÔÕÖ×ØÙÚâãäåæçèéêòóôõö÷øùúÿÚ\0\f\u0003\u0001\0\u0002\u0011\u0003\u0011\0?\0ýKU§T\u0011Ý\u0006\u008d˜©\u00188#¯4YßGt¬W\u008d½yÏQëSÌ¯k•Êí{\u00161K‘é@lÓXnïŽô6!8ô?\u009dE$HÝ¾¹4®‡±ýj¤›×ÿ\0×XJVèm\u0018ÝîI$1c\u0004~µVKhyÀ\u001fˆÍA4‡Ôõìj»9õ?\u009dpÊ¬oª:áMÛrÁ´‡p&5o¦Gõ§yQlÇ\u0090¡zã\u0018þUŸ$‡Þ¡i\u001f¹5ÊëF;#uIË©¡,Vøâ\u0014\u001cc¡þyª2\u0018ã<D€zì\u001fýz\u0081Ÿ=Éõ¤Ü©Ï?\u009dsJ²–Æñ¥Ë»¹#ßy|p=ö\u0081ý*´·E¹?Ê¬®¤‘\u007fË\u0015\u0090ûŠsk»pM´q¦pYˆ\u0003õ\u001eÕ\u001cñ¶²4Qié\u0013\"I7p©ŸaÖ©ÜéïÏ™\v'8ÜãhüÍo6¹%ÑÝ\u0013ÂÑç\u0018S“ø\u008f_jÏÕµ)î¬Ìq·‘Î\u001ab6ž\u000f@\u0001ï\u0081Î}k’§³³w:)Ê¥ÒµŽ~ãG\u008d±ÛÓ$b²®¼/æ†rƒ\u001dK3\u001ccëÒº[}H[¨\u00129p\u000e~QÏ§$“Ç°ÅAy®BÙýÛ’Aã\0\fþY®\u0006©I]³µN´]’9\u000føD|Æ>T–äŽr\u000ej\u008dÇ\u0081nåcºUaê ä~‚ºKÍ`I\u0016ÄI`ã\u001b£p\u000fçŠåï£Id;ÚåÁà†›?Ò¼úŽ”vGT\u001dio¡\u0081«x^ÏOÏÚ.0G\u0004î\n?\u009dqúœž\u001f·“\u0006}ç\u0004î'#ô\u0006»™,ì Ë¥Š;c\u001b¥\u001b\u008dR¸»\u0011ýÈ\u0011\u00069\n¸¯:u\u0015ôÐêWêy¼ßÙW\u007f,hä“€±†ÇàvŠ¥qá›\u0019£'ì“ƒœüÇ·\u001d2Gó®ÓR˜¾âK\0yÀéXWwLˆBñÛsr\u007f\nçöÍu)Å½Î\u0013Uð\u008d¢nU³–BFz¨\u001e¼ŒŸå\\Æ¥áÝ.Ü\u000f4K\u001bŸá\u0004\u001eþ¸®âú2ÛòXóžMrÚÄCŒ\u001eý«¢\u0015ßsžqHýQ\u008fË–/—\u0018nxª\u001a}’éòL3”‘‰\0zuÉþ_…Bf¸†äÄ‰æF8ùx\u0003ëïþ>Õ~F\u0012|‡\aóæ¿_ææÖÖgçöqÒú2T!WÛ¯'43\nÌ³Ôš÷¬~YÜÊ\u0014œž\t\u001f¯§j³ÈïQí.´\u001f%·Ü{ÊTõ¨Út=MG!à’Ø\u001fJ\u0081ö¶Fï¦?Æ°”Ú5ŒS$tGéÏÒ«¼#°Ï5CY¾‹M·Ie™b‹z†i\u0018\"Œ°\u0003œòrx\a®1Ås\u001a÷ˆ¯m5I`•ÖâÕ’X~Ã\u001eÔg\u001eZ\u0090Þfr>bS·$\u001e\0Éà«ˆ„_¼ŽÚ4gQû§DÚÎ›æ,\u007foµÞÓµ²\u00812å¦PIŒ\fòÀ\u0002JŽF\u000f\u00153(+¸r½ˆ<WŠø_â6…â%Š\rGM\u008doìîc¼\u0017ÐFBGq#\u0003\u0010;GÌ\u0018\u0018òÄà†\\’~jõý\u0013T±¼±Sf#¸·\u008dÚ3ä±e\u000e\t\f\u0014÷\0ä\u000f Ç\u0018®?i\u0019êž‡m|5L+åš\u001eÉ»8RGµck:ô:dÂ/(»\u0081–'=À \u000e}ýëbþ\v«„2Ä­\ní8Esþ}k–ºÐîµ\t\v”v$çv\u000f_­y˜š³‚å‚ÔÛ\v\u001asw¨ôE3ã)-ï\t\u008dL±ö\f\0ëŒöíÏøÖ\u001e­â+­B]ÒÈÅGÝS€\0ü;ŸZÕÔ|3ug\u0001‘ÁùFH\u0003 \u001dÏ`+\u001aßI’ñ‰\u0019#¸Ï5àÔ¯[à“gÐR†\u001fã\u008d´7¼#«\u008f.HK23¶C\u0011ÇAÀÎyïÒ¶¦’ÜÚÉ\u0012îÜNì7'Ž;Õ;]>\u001d.ÇË%·d6\u00063ž2>\u009d\u007f*F‘Üñ\u0018QõÏùë]0­(EFGŸQF¤Üã¢\u0018mÔô5RâÜd‘WvÊÙØ»±×hÍA7˜¼2\u0010}\u0018b³•M6\u000emw1®­Ï8¬›«Y3Åt²+\u001fáü†j³Z»1Ý\u001e1Ø\u000ek‚r¹ª©dr\u0013Y»u\u0004ÖeÕ˜ä>\a^¦»ù¢†8Èû6ãýâ§?­b\\E\u000eìùxÇ¶+†¦\u009dK\u008dvú\u001c\u0005îždÈ@Ì}†k\u0002ïE¹v!\"$žäW¤^L›]U\0\a®+\u001aéŽÜ\u0002@ö8®\u0019T³Ñ›ª\u008f±æ\u001aŽƒ|Ù\u0001\u0001$ãh<þ5Í_xGP’B<œŸO1\u007f™5ëw«¸\u001c\fõ\u001d3\\æ¡j[¾Þ{\fQ\u001aò½„í-ÏµìüG\u001c’\u0018P\u0005%¾fÀå°r@ü\aæ*OíH­#Žs,›Y“÷$\u001cá˜\"€IèXäç'žÜW=®}ƒK¸}H[I\r²’¬ÊÅFü\u0090\u0006Þ8'\u0003#9cùæëwW×Vn¶¬–·r\u0014`e\u0090\u0016ÀeÜr7`\u0081œ\u001f\\c\u001dGì\u0015+Ô‡™òÑÃB¥¹t=6\u0013\u001b9”*‚zà\u000fläþU1„IÈèzW›Zø‚ûR¾‡I²w¸Ý\u0017›s3&\"\u0003€£p\u001c19ùyà\u0013\u0081]î—\u001cöšt\u0011\\È¯*¨\fcéŸAÀÈ\u001d2@Ï\\\u000e•èá«*ôÔžŒó+Ñ•\u0019Y±/”F»?‹Ô}\reI!\u008dN8ô­=FîÞ\u001džt©\u0019‘¶©r\u0006I \u0001ŸÄ~u\u009dyjÊ\rrbn\u009dâoBÖJGŠ|Dø™\u0014\u001e*ºðÔ“Éov±$°5Ä`Û»HÊª²\0rÈY€À\0\u0090²\0rs\\¬:§ˆ­õ\u0003\u0011¹Žòé\u0090Î¿g]‚ÕÕ—1\0s\u0019\u008d2[\u0001•ˆ\b0Nwz\aÆ/„º_Å-\u0001¬õ;\bn|®—\r…’\u0018É\u0005Ê±\u001cg\0‘‘\u0090¸ÈÎGË¾;Õ®¼+¬éº5µµÕ¢I¦ÝFÚ=¼\u001bËÛ$Š¬P+eH\t4†Ff”•\u0090\u001c`\u0011óµ&å/3ì0°…X¨Ó²îvþ8:ë__Â·\u0081¡šê6ŽÙð\u0003¾vüÍ\u001c€•8PX`íeQ\u0081—\u001e§à¯ˆRé,ó^\\J “j½­º\tU]„)\u0019F\n¡c$8\fÁW –+\u0090£æ\u0006øÓ¡ê\u0016Osq%†¥!\u0002\tä’ÔÆt÷Þ¨Ä‡‰ÆåÌŒIÎ8\u0019;w\u001bñø²ÆÂ=\u000fQ:Uæ¾÷Ïö\u0011o¢°šÕ­Ÿk‹Ic2(\u0003\u000eØ\u008dW÷žL`åCF0NQIìÏc\u0011F\u0015©¨?É\u007f]\u000f¼ôím/ôË;è›uµÄi4r7VVPAÁ\u001c\f\u0010q×ŸÎù¸\v\u0010—\u0001\u0014ò\u0018ã“è\aø×”ü\u0016·ñ\u0004ž\u0013\u0011øžÒòËVù%¸ší“Ë‘œn\v\u0012«\u0012\u0002)U9U\u001b²\u0001$\u001cwr>Ü/˜\\/@zV¯\u0015*kÞ>\u001e¦\u001e\u0011›„]ìË²Éö\u0090Àü©×v?¦*ƒÚÄ¿ u^r@\a¯×\u00155ªÇ2’ìTŽÀÔŸf·“#\fÝó’\u0005rÊJªL\u0017¹¢*\u001b+q\u001eà7\u0011üG¦{ç¥V˜'#a'\u0081ÉÈ\u0018ì\aJÒÄp‚¨‡\u001dùªÏ\u000eúå¨ô´M£-ued¸1ð\u0015Hé‚*9/¤ç\0\u000fóÒ¬\u001b3ÛŠ\u008d¬\u008fÖ¸åR¥¬Š¼[ÔÏšây\a<\u000fN+>}ìpsøVÄ–.\u000f\nj¬–n3ÃW\u0015IMî5(ô0.#$\u0011“øÖMÕ¾ìƒÏ­t×\u0016ÿ\0)àç¿5“u\u000f^+Í©{êm\u001a‹¡Ì]Yõ¬{Ë>ÿ\0ZéîãÂšÆº_nõÄís®3os™¹„¦qXwÑ¾îµÔÝF§<V5Ô)Ó\u0015q]My\u008fqÓï\a‰mæÓ5VS\u0019ÜÂ0û\u001d†C\u0006\u0004\u001eŠ}±‘‚89ó›¯\u0011Ûøw\\þÖ—T™à‘J\u0019_`MÃ ¨P21€H'$°\u0018\u0018$·_ðõî»ªE\r\u009dÜ¶\u0006îá­\u0019æµ\r\u00032«± ä\u001cìR\n€A\u001e€\u001càÜøGC“ÅÖÚ?‰lá¸·µ»]ÑÅ!]Ëä³–$\u0010À\u0016\u0011\u001c\u001eN\ba‚3ú„äÓNR/\u000f\u001atùœµòî{\u008f‡|me£ivÈÁž\u0015\u0019Þ¹,\u0001ÁÉ\u001dÎO \u001e½\u008fZô+?\u0010A3\"³€Î>MÇ\u0019ÇaïíÖ¾Iµ×\u0006íbÄJ×ðÁ$ÐÚÉ#(ócÝ…v\u0003\0| ŸS»¾\b=V‘ñcNðÏ†¯%Õ ¹¶·³•`UBd%”\u000fš<€\u0006@\u0004(ï\u0090@\0ç\u007f­N\u008dšØóg\u0081X‡h-OuÔ—Oñ^¡n>ÐÈ4é\u0016YUv\u0090êÙÚ\u000f ,ƒß\u0019\u001dò:—Q2qÒ¼šK\u0013©hwÚ\\7pZê\u0017\u0012‰\u0016í×\t;d!\r€pyÆ;\u00121Ü\u000fDðí\u008fö&•a§‰šqk\u0004py’\u000f™ö¨]Ç“Ï\u0019üz×n\u001e·´\u008då­Ï#\u0011KÙ¨Ùêº\u0016ÿ\0³\vI¸r+Àþ'ü=»ŽmLÙÝG¹\u0004s¾˜¨¹»\u001bÙ£óX\u0090J†ûA\t\u0090¬e \u0010T5})\0\u001bA¯7øÍ£Ýÿ\0gÇ¨iQ+ßË5¥¦9\u001e`ûJlR@8\u0004»Œô\u001b¹ã5¾#\u0003\u0019ÒºÜÏ\a‹të%-\u008fŒümð÷Æ¶ž>¸×¤šÖòÂùÊZ_XØ†¾s\u0018˜ÂÍ\0dW!gvùX\tV\u0012 FÏ\u001a\u001fNýžþ\u0017\u001d*Æ\v½NuÔ.íRÞ\rñBcƒ|AÊ´g\0œ‰\u0001|à\u0019#Î\08\u001e‹ñ\u008fF“F×|5m¥Ù,·Ú‡\u009d\u0015”m‘\u001f™\u001d¼¥A8!r_\u0004õÁcÐ\u001aÔŒÁð÷O±²\u0016îñÃ\u001a,­\r¬’¼Ò\u001e\t\u001b\u0014““’XŒ\rÀ\f“ÇÊâ©Tæähúyf\u001cÔR‹Üéõ\u001dRßH´iî™’\u0014RÌÀŒ(\0’NHãŒ{’\0É TÖ÷Ö×³<PH%e\u008deÜ™+µ‹\0AèyFéè\u000fq^WñCÄ2\u007fgÅ¬iw\u0012ÜØycÍ’\u0002±ˆÕ˜ªd’­‚À€\u0018\u0010\t8 œ\u0015ø\u0013 ë°ÜÝê·æ{]9áò!µšRÆG\f2ì7\u001c\u0014!Æq–2\u0012pA­%‡§\u001c7µ“³8=‹ö^Õ¿‘ì\u0010â6\u0019S×éSÈ»¹\u001c\u001eÕ\u0017_z{J#\u008d\u009d\u009dU\0\u0004³\u0010\0\u001e¤ŸækÉRÒÇ\f·¹\u0018\u0081Û ÏÐTžQ\u008f\u0019\u0003ó¬í7Åš~©«^i\u0090Jßlµ#tm\u001b)`@%—#•\u0019\0žÇƒŒŠ¯wã\u008d\u001fN×#ÒnîJÞÈÁQ\u00126bÄ€qÀ<à\u008f¯Qß\u0002ƒû:°—:v’±º#_ò)¬£°¨¾Ôæòx¼¢‘ÆªË!Î\u001f9È\a\u0018È# $ò:\u00022ö˜í'§ëXJ¿.ŒÅ¦G.Õ_OÆ³n.Sv9Ç©È«’É»\u001bÎ}j¬Â28\u001f\u009dpÔ¬êk\u001d‡\u0014–å\t§;r0;\nÇ¼fl÷ü+Nå\a<ãéYsqÇZäæ”Ý™¼UµF\u001dä}k\u0012é6æº[¥\u001cñúV\u001dô{º\fþ\u0015\u0012Ã½ìuFg5v:ñëY\u0013\u0001Ÿz×Ô¡ŸÌB‡\b\u0018ï\u0004u\u001bHÇ·8?\u009de\\Æ{ŽôF‹Oc²2V)ØxûX¹Ò´¯ìû¥i-nŒŒó–\u0001•Î\u001f\u0018\a'\u000eøÏ\\\u0003ØÖeÊê>0\u0006{»…–ýÂ•ó\u000eÒÛA\u0005AûÛI\u0018ç\u001c\u0002O'žká\u000e¡\u0013¤±Nþdq‘(<6òÃå(Gmª˜\u001d2ÝN@­Íz;k\u007f\u0011Ew)C#H6Ê¬Àc%Š‘\u009d£ \u0003¹ôÈ¯Ññ\u0010©)r¦Z©\v¶•Ž\u0013\\ðî¯¨k\u0016ÞUËÃ<.\u0012âu—lEFK…\u0019 °à{`dœ\u0001^ÃáËT0Ù\u0019âûdvò$ñ,™t\u0013.Ü8b\n–\u0004\u0010\t ‚3×\u0015çzÀ¹Ôongœ±‰\u0001e\u0016ç\fà\f\u00102:äœàðyê+\nêy¬n¡F–CbÌ$hÑˆ\u001bY‰9bx\u0018Ú9ä’9\u0019\u0002¢¥\u0019V¤¡)Xj§+æ‰î\u001e*ø‰i¢ÚÙÙ¢G\u0015åÅÄ‹±ÁL.öbå”pªß1õ\u0001¸9\0öß\u0002~8Çâ+8ô\u008doPYµ4y\u0016\vîDw($!@-Œ¸Á\u001d9\nrI\u0006¾u³ñ\u0015¿Št{Û95x–æ\u0015Ž1r“¨ä>åó\u000eHe\f«» \u0012¬\u007fˆn¬Í+ÅZ„>\u001cÖlôë+{‹E”5”È¥á´”\u009d²là…\u0003ç(Tƒ•ç©\u0015äà]L<\u009d7ºg~&\u009d:ø%îl÷ý\u000fÑ«-Q\u0019GÍ•õ®sâ\u0016‡kâ½ Ûë\v¦¾•ö¸#–\rX\a·–\u0016uY\u0015\u0090‚¬Î¬Ñ¨n2Àà\u0090\u0001ù{á/ÄÍ_Z³H\u009dÔK\u001b\u008dêÄä©à0\u0019à\u001eý²\u000e8ÅWý­>.Z^|-³·²“ûORÒõ€ÓéöîVn\"–,í$\u0012\u0015\u009dI8 \u00100rA¯´ÃV”Ÿ³‘ñòÁòÊñv8ïÚ+á/Å\u008f…\u007f\u0012t\u007f\u0014ü;ñ~©{ é²}ª-'ZÖå»·²r®…c‚RÀ!ˆ²ü¸`®à6\t\u0015Ì|-øß«xÒêM'Ä\u0012êW\u001aÊÅlÐ]²Âgºk›€\"û,’\u0002$DPä\u0017À'Ë\u0019\u0001ˆ¯røûãm\vÅšf«c\u0006§kv²YH®-n–FPÊÊ\u0018…c…\u0004ç'\u0081ƒ’9ÇŠü)×<)ð÷Ãúdw÷\u0016SÈ¶zvœšƒ…p«\n’e\u0004ƒåðb“i$€\u0014\u0090p\rg]Ñ³ç~ò5ÖIE+yžåðƒBñ\u0017€<1¯kž:¿š-\nÞÚH­íæc3=¸XÔOq\u001c{²à#d©`æGm \u009dÏèŸ\u0002~*x\u007fâ×‚$Õ<3\u0014Ñi¶—riß¼Œ\"—Œ)>P\u0004þìn\u0001I\0à\u000e\u0006\u0005r—\u001e0]sKºHîlu}\"â&¶žßxž9£d\u0001‘€$\u0010QˆäôaØ’SÀ~2ð¯Ãí)4=\u000fÂÖÞ\u0016³2–dÓíÑ-Ì„\0dr0rBŒ–\u0004à\f“Ž>_\u0011Ju¹¦Ö½–ÇDiJ1åOCØ5í÷:Må¤7/iq</\u0012\\G\u008dÑ3)\u0001‡#\u0090H#ÜWœè/¨_|&Ñc}oV]]­b¹7·P´W^c\u0002î\u001a9“#\u001bÊ„‘2\0\0ŒŒ‹3øÒ\v«„\u008dgY^Q”XÆðFÕnH<|¬\u000f=A\u0018ÎFYyp÷™$Æ\tà\u0005`;zdÿ\0œ\nš8VâÔ\u008dÕ?fÔ™å?\u0003[Äž(ø™qâ[»”´ðå¢°D…D-rí“\u001aíV8UV\u0004’\u0002± .åÉ¯qÔ.¬[ÆÚ}òh«4æÞe“W\u008fËS\u0001\u001b6«eƒ\u001c€T\u0010\u000e\0 \u0090\t\açÏ…Ú÷‹u\u001f\u001cxæÃY´»³Ñ-'UÓçm=­Ò`Z@LEˆ\f\u0002¤cåÊ€\u0014ç$³ú%©½ŠÖ(âÞ…#\nv“(ÈQ\u0090\u0018€N\0ê@'®\aAÛõ=o°ªMVjLõ†ñ\"\u0016À“žƒšzøŠ\u0016Œe×\u0018ÎZ¼{\u0017²ï\u0002ço\u0004¶â\acÎ}±ØT°ø'UºU–\t$”\u0015\f|¸™óžç\u0003<òAïÍc,¶3V2|–=E¼]§Èè‚uB\tÜÏÂà\u0003Ð“€sƒÏjš×T¶Õ!ó­'IÓ8%\b8>†¼\u001fÅž\u0019Ôô¿-nDÖˆùeÊ‘»\u0081íÛÓÞ¸I¤Ôt©üØ.\u0018\u0015\u001fx‚xÎpGOÒ¹žR½\fÜcm\u000fªîœ\"3³\u0004P2YŽ\u0006>µçž*ñâY\u0013öl”\u0003\u0002EèI8\u001cûžÝx¯&\u007f‰ú¥ê„½¸w\br\u0003\u0012A=22\u007fúþõÎëZ„šÂ\u0002\u001f \u0013•^\u0018\u000f§8\u0014èå^ÎW˜£d®z\u001dçÄbÐÈÏ{5¾\u0014‘¸¶\u000fÐŒ×1mñqãÔ\u0011ÞæibÞ\vG!\u00040Æ\b\u0019>„ô=@¯;¾ÑïçUR.\u001a!\u009dÄÆO\u001dðzdž3\\õöƒw:\u0081ö–\bÄŒ\u0005ÁÀÎs\u0081Éæ½¸àéµk\u0013*–Ù\u001fNÚø³LÖ­EÅ¬›ÐŒ•$nCè@'\aôôâ±µ?\u0011@¬\u0004\u001f7«\u0010?Æ¾l’Æ[a³íä18\u0011H¸\u0003Ü\u001e£>àÔ©©êp\u001d±ê²aF6oÜG=·\u0003Åaý—\u001dÐ*öÝ\u001f`[ü3ðÅ½Š\\iÖ©£]©Þc…™Pp\u0015P¡À\u0005T 8È\f\u001b9 \u001aÎ¼ø[{â\u00056ö¥/K°\f¸*3×9éÇ'<~§?\bø7ãçŽ|/©Z\u0004¾ÔuüÌ¢êÜjgeÐuË\u0001\u001c¥Š•;Á(0N\0Ã\u0011·ë?\tþÜ>\u001bÑ|?á¿µi÷–º•ú³]Ø£ ’ÉW‚Ïæ\u0015,\tÎ\bÆ@Èç\u0002µ§YMûêÁ\f\\&\u009d\u009d™ëzOìÂfµ‘µ[ù#w`Â\u001bfùW©$1\u001b‰>‡\u0081ÓœW\u001fñ_öd\u001fØ“Ë¢ê$_À\f\u0090%ÀùC\fsòŒƒ\u0081€pp\u000f\u00038¯dðŸÅm#ÆÚ5®­£ê\u007fÚ–R\u0002Œ…ŠÉ\fƒ\u001b•—\u0019\u0004g\u0004\u0011žA\u0004Œ\u001aÓÔ¯žö\u0016W\u008d\u0012\"\u000eIåˆÇ<Ÿð®ÛÂÉÄÖ2¨õoF~qx\u001fÀºî\u009dã3o\u007ffÅf’;[©\u0010–\r\u001b«‡ \u0003¸\u001e\n©Æ\u007fx3‚r¾Ùâ\u000f\0}ž\u001b;m.!¥O\u0013\0#Ž5ˆH˜\u0018$\u0005;œ`…$‘—lðA\u001eéáM\u0017Hó…áÓ-\u001e[†`³´ ?ËÈ9#‘\u0090pxÎzð\u0005\\ñF‹§Cro\n#\\D0\u0015—#Û\u0003Ö¸êP\u008dJŠ¶Ìè\u008dg\u0015ìú\u001e1àŸ\fjKjoô«håÕ!‰§¸µ\u008d¤Ä’m8\u001c\u0003Œ‘€\u0001\0dc¶8o\u0016]k\u001aN¤l¬­£¼–7Ú‹k\u001bJ²³mÚƒq,\t\u0004`0\u0004‚\u0006\u0001Í{í¯Œ­´ù.`x¿³¡‘DrßG<P:î\u0004ä`ä`¨o™†B“‚\u0003\nà5ÔÑî¬eÔç2Ý¢ÝÅ¹­QX£°\u0090.ÇFL\u009dÀ+7-”\u0004±^\u0006ªœc-w1öÜ÷GÌ^(ñf¡g Çª‰c€1Ì»YTÊ¸*\0\u0019\f«—`v…n[\u0004\u0010I¿á=#UøÅ¤‹/\u0004E\u0006…8wf7Ù‰\u001a0\u001a< Tb@\u0013\u0010A\0°-Œc\u0015ÕYþÏ·z½ìqy­\"FY–Yc,»K\u0002\0,\u0003\u00122Àä``\u0081\u0090A?Q~Ï\u007f\u0006ôÿ\0\aÅ.§:E$ûZ\b£Ú\u0002ª’3\u0081ÀÉ#ÓŽsÉ9ÝQ¥\u0017u«1³\u008däÎ'áÏ…5ÿ\0\u0003x\u001fOÒ5¹âÔ5\u0018C,ÓFÌñä1\b\u0001`\u0019‚ A’3Áè0*Î¡¦Üê\u0013*\u0010\u0004±€à4kŒäd’=¸ïœs^¿ãŸ\u000eÅ\u0012½Å¼±Â\u0017“\b9$’\u0006\0ÏAè++Áþ\u001b\u001aôÒ½ÆaŠ5Ï˜@\u0019'€\u0006F\u000f~ŸÖ²tß5Ñ¬j$®y\f0ê­ãûsvl`K\u0015\u0013Iq$QÄ\u001d\u0003FDQÆ\u001b,ÎdÉ%”\u0002\ver\u0010}\u0003¤i:>½¦Zj\u0090Û½½•ÂnDv`ì\t8nàä`‚2\b äƒšòÌËqñ?NÑî|+©K¥ÞÜÆeÔŒ\r\u0014+å¦ÐÌÀ• ‰\u001dJ’\u0001\u0005HÉŒ­}\u0005sgeä¤)*À\u0006\u0015T?`0\0\u0019À\0cð¬°´åy?²i_\u0011\u001a‘Š[£*Ö\u001d\u001fAÄðÚbU\u001f~dÜz`àž„ôã\u001d{t®’ØÆ¶Ñ´iå\u0017\u001b¹P:Žà~µÊÞh°]yÖïw3¸PII1ƒž0:\u001eÜ\u001eÕ…yâ«ý>ch†K‡Æ\u0016hÔ•Æ2@8ë€xÏ\u001d{Wz—.ç\u001c£Ï±ÝM¯Gc1\u0090G\u0013ÊFÀ6€G<{‘žßÊµ„FâÞ9^$I\u0019A\u0003¦>½¿\u0003é^_¡I~d:\u009däYE'\v!Á8Ç \u001fÐVÎ¡ñ2ÒÚDLžx\u0001›\u001f\\qÆ1Ò¶…M=ã\u0019Òw\\§W¨h\u0016WÖä]G\u001dÀ#\u0005YC\u000eF8\aŽžµæú÷Â\u009d\u0002áæH¢’Ùßî1”°\u001cuÁ<œúœ}:ÖÝ¯\u008f Ô!wŠò0‹œª¸Î?úØ®\u007fWñ\u0012_°[i¥’R2»I\r×©\a·¿OJ'(Ih‰Œg\u0017«<ŸÆ~\u0005±Ño×Èß¨F„\u0017Œ\r­“ß#·§N=jžŸkwf¯\u0006\u009dáûx<É<Ù&ºvv*1…\u0018<c‘Æ@Î}sêºW‡¾ÒÆKŸÞ–9fqÔÿ\0‘Ò®Ýèñ#\u0011\u0014\0q\u008dÁqúÖj=M\u001c\u008f\u001f“Ã7þ$»I5s\u0019HÀQ\u0014\0ªã\u00189é’{“íŒbºm7G¶Ðá)\u0015´H¼ýÕ\0óïŒþµÔ\u007feì?\"wÏJ£¨Ú\bÐŸ­R\u008dŒÛ<ŸÇ^\u0003°Õn’w·H\u009dÉ-$ySœõ$c?\u008dy»xgDð…Ó\u008dA&Ô]ó²I\u0013Ì\u0018ã\u008d¹\u0003\u008f_zö/\u0012^~å÷0\u0001sŽkç\u008f‰:åÃÝ\"ÛÈÒ6OÜ9À­b›vFrj×>\u001b\u008fSÔï4ûÅ´¿IÖY\u001a9,n.×Êr¤€cNAà‚\b )\u0019ì\r{7Ž¯,åø[à‹oµË\u0002i±L—‘­´¬ö³I ‘ä\u0018\u0006=ˆ«…`Á\u0081v\u001bH\u0004×%\u001czdÖŒò‚\u001dJ 38`p\t$:°\u001bA\u0003 \u000e€\u0002r\t\u001a7z×‡4½-\u0081½¹Šâ@T¬ŒUA*\u0017\f\u0001,\t'\u00047\u001c‚8\0\u000eZ\u008fšK–;\u001cj›Š—™í¿²\u008fÇ…ð5Ï‰o'»\u0003ÃðÚ-ÅÉ˜HÓK\"+\bÖ$Lª³\u0013‚Ì\0 `\u0005 Šõ\v\u001fÛãTÕ—V²—J‹Í‚ÞtÙo¼H“7ü{‡l²\u0004`®\f™ÚXÆ\u0001PÀŸŽ|Eâï\u000fj^\u001f{kMRÖÃRM“,’E*!‘H >Ô\0\u001cŒ“‚20xéÊÍã¨-|'s\0p—\u0013Å*\u0014Œ\u008dí+\u0090¦Ve',v!eÁ\u001dF@àå\u001a\u0013ÑÛ©½<Dé¥\u001bè\u008f×\u008f‡¿\u0015<1â}\u0017N\u001a6¯\u0016¡\u0005”K\u0019e?¾\u0001r…¤N \u009d„ƒ€\bÉ\u0019\u0004\u001aê5\u009dJ\u001drÆo.MäÆJH\u0001\u0004œ\u001e½ø<qé_•¿±gŠobøÁgf÷÷\u0012ÛÍm<{K°\u0018\bA.:0%Pàä\u0006ÁÎA\u0015ú\u001f§\\]éÛÄ·må‘•\u008d˜àwã°ÎkJ´ý›±éS©íb¥cÎ¾&KsáËÍ;ÄbßP–}>XåžÞÆxãÞˆÌÙÃpÃkH0\u000eA`\u0006\u0006HÜøiñ\vJñv\u008d§Ü^Mý¯l–ÑÀ×2ÀVC°ch9ùðÁ\u0081b\u0006J‚\u00060N§Å\u000f°ßxOW¿¾XLVÖ’JþnÐ›QK\u001cä€\tÀç¿\0ðjßÁ\u007f\fÙx[ÃòCªiñj÷×r‰VHÕ^5\u008fh\t\u0018,p0 \u0003´c9Á#\u0006´rN–»\u008dC–|ÉènéšÄ\u001fl²²Ñ-¤\a\u0018\r!$í<“Œ\u000eƒ=\u0005z×†ì&\u00817Ï!w-ó2\u0012\0\u001d€\u0019 wüs^~º”p\\;é–Vºr6\u0003\bcPì\a@p£\u001cóŽk¨Ñu‹\u008f²–y\u0019A$\u0090ztÇNý?JæVNæ²wV=&Ýí\u0015\u0015>Ï\u0016Þ¹ÆIéÉ$u5sÎ\u008dS÷QFª\u00060\0\u001czt¯*\u001e2òYÔÈeE=Tä\u008fozÔ±ñ ¼ˆ‘#\fòU\u008f?çŠéU,qÊ›Üï&¼ƒïÉ·…ÝÉÎ;g\u0015\rÍå´°ù²íò‚î2tÀ#9æ¹)¦2Û»‰3…8\u0004ûW$Þ.–ÞCmr_ËáB–ãò¤êy\u0015\u0018_©ÞÜ[Ù\\9›L\u009d‹±Ã„n3Û#ëï\\üš\u0015Ý¯î§œ\u001bds&øønF\bÉÏ\u0003žžµÌ\rzæßU2$\u008d\u0014c\u0004/ðžsÏ±üq×­m¶ªú…©\u007f5÷mÚ#g'ó÷÷¨ÒZØÕ'\u001d\u0003Tº\f©\u0019»¹tbIç\0\0=€\u001dÿ\0\u0013^GãÏ\u001cip-õ\u0090»RðÆÌ²g\r‘Áôä\u001cr?Z»âý#ÅúÃ$\u001adööáŽÒÒLË´\u001eã\br{sŽ½k•ÿ\0†o\u008fR×tËÍSR\u00170[ÜC=ÒI¹¾Ð\u0011·4x\u0004\0­€\b9\u0004\u00121ZF)êØ6ÒÐÝø\u001a¾ ×£žãì²[il\a—u&CMÇ\u0005AÎG¿CÛ=½»Kðß—‰e?¼ÆÜž¿çŠÚðÿ\0Ø#³S\u001c^R*ãæP \u0001žžƒò\u0014Ë=J-keí´ð¦š…€Ý\u009dòàã8ì\tÉ\aœ‚+U\u0014ÝÎiMìF¶\t\f&IH\u008d\u0014\u0012YŽ\0\u001f^Ø®-<hš–©<\u0016qÇ%šáVc‘“Œñùúv÷áß\u0013<Hž$X´}*I\u0004JÛ®%QµX`Œ\u0003ß<{}kš\u008fC—KŽ7ˆlòù,¹\u001f•T·Ð…{j]ñ'\u008d—A¸ò„bIT\u0002cÁ$çƒÏA\u008fzÀO\u001cGªZK=ÀŽ\u0014PAU$\u0090z\u0001ïŸoZ©}£¾¡pKýâÅ¤f'<öÏµeÜèaX¢€±Ž€t¤\a7âK©õÙ<‹(ÝarKHã\a\u001eƒŸÖ¹Öøj\u001bælŸ|\u000fð¯^³ÐÏ–ŽQ#\u008f\0p9lwÿ\0ët¢ãM\u0012±ÄxU8,F>˜ªIîCkcñ»O’ÓKkyncyÝ\u0018ü«(\u0003k\u0002\0\0c,\t''ŒãŽõša{Å&`làV'\u0005Ç\u0018À\0Œ\fœddpN8À9ÙÖôs\fÒ£A\u001aÉ\u0019ùûð=9ª\r\räÓ,r¹*ß0@ÝG¿?çÐu®õ\u001bjqÉô!g¶ŽáâÈ1«cÌ‰ÆA\u0018#\u0003'‘Ù²NyÉ\u0003\u0015WRºK…ŠÞ\"|»h‚üßÞÇ8Æ\a\u001càúzÔ·V&>r\u0004œ‚ÀuÏ_çYf)‚ÈäõcžkE\u0014g&ìz'ìçâ\u000fì/\u008dž\u0015¸2\b\u0003\\\u0018\u0015Û³É\u001bF¼žûœ\u000e99¯Ò}gÅÏ«ÛG\aš¹BNô?7¦3ýkò¯áì’Çñ\u001bÂ²¢ù¥5K\\!lg÷ªqžß_ÿ\0U}½\u001fŒgÒï&\u0019%\u000fÍ·Õ{þŸJç¯KžI£³\vUÆ-\u001eQûAüXñ\u0004šæ·áë»»Ï°[\\ˆÍ¡œ\"\u0015ÉeÊíÃ!\u008d\u0090üÀ“\u0090w\u0011Åz\u008fì\u007fñ³Ä:¶ ž\u001f¹ÔR]\u001bM°‘­cv\u0012Ï\u0019Y£P<à\u0001 \al\u0002\bÃ(\u0018\u0003\u0014š\u008fÂ¯\b|tÕ$›]:\u009d­ÜP\u0006[½>uG\u0090/\u0001\u001c:²Ÿ½×\0à\0I\u0001@±à\u001f…¾\u0017ø?­Þ\u001f\u000e}¾qp‚/?P•^]¹\u0004\u008f•\u0014\u000fáÎ\u0006\u000eÕ#\u001cŠåtéòòõ-s©óßCì\u008f\vëIyâ\bÞGO*bÒ\u0012\u0006\08Î?Ã>•Øx£RF… ƒo–0w/<ú\u007fŸZùƒCñ¥Æ“q\u001b–fˆžV½+Oñ‚j›L/#G´\u0013¼cëþ}ëŠPqgg:z\u009d^ã$ÃaÎ\b;\u0081ÉÏ|Ö¶“wöy²åWÔž+/Ãë\u0015ÂµÌï¶\u0011ó\u0015PIéŸóŠl×\u001fÚ\u0017\f-\u0015£‹¢³\u0011“ø\u007f\u008dEŠæ¾‡Q7‰E¿È›ONœó\\\u008fˆ/ŒŒÒ¦ÒêÙÆ3ƒÍC¨%ÎŸ\t™ß \u000eƒ×\u0015‹á»É5ËÉ\u0003\u0090!\u008f±\u001cžqþ5i_R¶fÍŽ¡-äbYÝ\u008f@\u0017\u001cý\u0005všL—7Kå*4pg;ˆäþ5\u008dc¥ÄÌ…S¡ãšô½\u001fKò!\r&\0þêÕ%v)KK”ìtxöƒ ,Of­4Óm\"%ŠmÇ'Ó\u0003šµ\u001ccqùqŒãšËÖ$s\tD8,0MmdŒ.Ù‹âï\u0011K}\u001bhÚ~Ò“\r³È™;Aê Ž9\u0003\a9ê}+6M\u001civ(\u0006d•°\u00166?^qß\u001c~uÑøgÃˆ»çlnÎIëÒ\u009du¥ù×\u0012\\.\u0006>Tö\u001d?ÏÖ\u009d¯©7KC:ËI\u008fLE\u0090\u008f6V8;±Ôöâ­ë\u0011¢Ûç`\b \u0012{\u0016ô\u001fýoJšK7•mâ\u008dË*±Ý“ŒàãüjMj14‰\u0010\u0018E9ÇÓ9­b´2rÕ\u001cRÚù™\f¿3\u0012OÖ¨ÜXª>6ry8\u001d+¤_.2d\nXçïg\u001e£¥dÜ|Ó;\u000f|æŸ*\u0015õ2X¼wh\u001f\u00902\aÒ¯\\4a9#<}ÓŒÖmìÂ9Œ¼ï\0ñÛ¯ÿ\0®³nuBÓs\u0090*–†2ÔÿÙ";
                byte[] bytearray = System.Text.Encoding.Default.GetBytes(x);
                BitmapImage bitmapimage = ByteArrayToImage(bytearray);
                Cover = bitmapimage;

                OnPropertyChanged(); } }

        private  AsyncObservableCollection<Route> _detailRouteList;
        public AsyncObservableCollection<Route> DetailRouteList { get => _detailRouteList; set { _detailRouteList = value; OnPropertyChanged(); } }

        private AsyncObservableCollection<Member> _detailMemberList;
        public AsyncObservableCollection<Member> DetailMemberList { get => _detailMemberList; set { _detailMemberList = value; OnPropertyChanged(); } }

        private string _startDate;
        public string StartDate { get => _startDate; set { _startDate = value; OnPropertyChanged(); } }

        private string _endDate;
        public string EndDate { get => _endDate; set { _endDate = value; OnPropertyChanged(); } }

        private BitmapImage _cover;
        public BitmapImage Cover { get => _cover; set { _cover = value; OnPropertyChanged(); } }

        //Commands
        public ICommand CloseWindowCommand { get; set; }
        

        public DetailUCViewModel()
        {
            int JourneyId = 1;
            DetailJourney = DataProvider.Ins.DB.Journeys.Where(x => x.Id == JourneyId).FirstOrDefault();
        }



        public BitmapImage ByteArrayToImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
