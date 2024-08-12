import os.path

# tModLoader .lang file updater by jopojelly. (v1.1)
# Run this script after updating en-US.lang with new keys. python 3.
# Also make sure the file encodings are UTF-8 not UTF-8-BOM.
# You can use this script for your own mod if you credit me.

filename = './{0}.lang'

languages = ['zh-Hans', 'ru-RU', 'pt-BR', 'pl-PL', 'it-IT', 'fr-FR', 'es-ES', 'de-DE']
missings = []
for language in languages:
    if not os.path.isfile(filename.format(language)):
        with open(filename.format(language), 'w', encoding='utf-8') as output:
            output.write("# Translation by: \n\nmissing=missing")
        #continue
    #language = 'zh-Hans'
    otherLanguage = ''
    missing = 0
    #print("Updating:",language)
    with open(filename.format('en-US'), 'r', encoding='utf-8') as english, open(filename.format(language), 'r', encoding='utf-8') as other:
        enLines = english.readlines()
        
        # Preserve Credits (comment lines on first few lines)
        otherLinesAll = other.readlines()

        for otherLine in otherLinesAll:
            if otherLine.startswith('#'):
                otherLanguage += otherLine
            else:
                break
                
        # Skip empty whitespace and comment lines to end up with only json lines.
        otherLines = [x for x in otherLinesAll if not (x.startswith("#") or len(x.strip()) == 0)]

        if len(otherLines) == 0:
            otherLines.append('TrashEntry') # just to prevent Index out of Range
        otherIndex = 0
        engComments = True
        for englishIndex, englishLine in enumerate(enLines):
            if englishLine.startswith('#') and engComments:
                continue
            if len(englishLine.strip()) == 0 and engComments:
                engComments = False
            
            # For lines with key values pairs, copy translation or add commented translation placeholder.
            if englishLine.find("=") != -1:
                if len(otherLines) > otherIndex and otherLines[otherIndex].startswith(englishLine[:englishLine.find("=")]):
                    otherLanguage += otherLines[otherIndex]
                    otherIndex += 1
                else:
                    otherLanguage += "# " + englishLine.strip() + '\n'
                    missing += 1
            # Add English Comments back in
            elif englishLine.strip().startswith('#'):
                otherLanguage += englishLine
            # Add other json lines in. Also add in whitespace lines.
            else:
                otherLanguage += englishLine
                if len(englishLine.strip()) > 0:
                    otherIndex += 1
            #print(otherLanguage)
    # Save changes.
    if missing > 0:
        missings.append( (language, missing) )
        print(language,missing)
    with open(filename.format(language), 'w', encoding='utf-8') as output:
        output.write(otherLanguage)

with open('./TranslationsNeeded.txt', 'w', encoding='utf-8') as output:
    if len(missings) == 0:
        output.write('All Translations up-to-date!')
    else:
        for entry in missings: 
            output.write(str(entry[0]) + " " + str(entry[1]) + "\n")
#print("Make sure to run Diff.")
#input("Press Enter to continue...")