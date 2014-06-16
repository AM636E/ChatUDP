<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="html" indent="yes"/>

  <xsl:template match="/">
    <html>
      <body>
        <table border="1" cellpadding="4" cellspacing="0">
          <tr bgcolor="#999999" align="center">
            <th>Name</th>
            <th>Price</th>
          </tr>
          <xsl:for-each select="LIBRARY/BOOK">
            <xsl:sort select="library/price"/>
            <tr>
              <td>
                <xsl:value-of select="NAME"/>
              </td>
              <td>
                <xsl:value-of select="PRICE"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
