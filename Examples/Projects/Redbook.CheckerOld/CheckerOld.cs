#region License
/*
BSD License
Copyright �2003-2004 Randy Ridge
http://www.randyridge.com/Tao/Default.aspx
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions
are met:

1. Redistributions of source code must retain the above copyright notice,
   this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

3. Neither Randy Ridge nor the names of any Tao contributors may be used to
   endorse or promote products derived from this software without specific
   prior written permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
   FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
   COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
   INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
   BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
   LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
   CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
   LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN
   ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
   POSSIBILITY OF SUCH DAMAGE.
*/
#endregion License

#region Original Credits / License
/* Copyright (c) Mark J. Kilgard, 1994. */
/*
 * (c) Copyright 1993, Silicon Graphics, Inc.
 * ALL RIGHTS RESERVED 
 * Permission to use, copy, modify, and distribute this software for 
 * any purpose and without fee is hereby granted, provided that the above
 * copyright notice appear in all copies and that both the copyright notice
 * and this permission notice appear in supporting documentation, and that 
 * the name of Silicon Graphics, Inc. not be used in advertising
 * or publicity pertaining to distribution of the software without specific,
 * written prior permission. 
 *
 * THE MATERIAL EMBODIED ON THIS SOFTWARE IS PROVIDED TO YOU "AS-IS"
 * AND WITHOUT WARRANTY OF ANY KIND, EXPRESS, IMPLIED OR OTHERWISE,
 * INCLUDING WITHOUT LIMITATION, ANY WARRANTY OF MERCHANTABILITY OR
 * FITNESS FOR A PARTICULAR PURPOSE.  IN NO EVENT SHALL SILICON
 * GRAPHICS, INC.  BE LIABLE TO YOU OR ANYONE ELSE FOR ANY DIRECT,
 * SPECIAL, INCIDENTAL, INDIRECT OR CONSEQUENTIAL DAMAGES OF ANY
 * KIND, OR ANY DAMAGES WHATSOEVER, INCLUDING WITHOUT LIMITATION,
 * LOSS OF PROFIT, LOSS OF USE, SAVINGS OR REVENUE, OR THE CLAIMS OF
 * THIRD PARTIES, WHETHER OR NOT SILICON GRAPHICS, INC.  HAS BEEN
 * ADVISED OF THE POSSIBILITY OF SUCH LOSS, HOWEVER CAUSED AND ON
 * ANY THEORY OF LIABILITY, ARISING OUT OF OR IN CONNECTION WITH THE
 * POSSESSION, USE OR PERFORMANCE OF THIS SOFTWARE.
 * 
 * US Government Users Restricted Rights 
 * Use, duplication, or disclosure by the Government is subject to
 * restrictions set forth in FAR 52.227.19(c)(2) or subparagraph
 * (c)(1)(ii) of the Rights in Technical Data and Computer Software
 * clause at DFARS 252.227-7013 and/or in similar or successor
 * clauses in the FAR or the DOD or NASA FAR Supplement.
 * Unpublished-- rights reserved under the copyright laws of the
 * United States.  Contractor/manufacturer is Silicon Graphics,
 * Inc., 2011 N.  Shoreline Blvd., Mountain View, CA 94039-7311.
 *
 * OpenGL(TM) is a trademark of Silicon Graphics, Inc.
 */
#endregion Original Credits / License

using System;
using Tao.Glut;
using Tao.OpenGl;

namespace Redbook {
    #region Class Documentation
    /// <summary>
    ///     This program texture maps a checkerboard image onto two rectangles.  This program
    ///     clamps the texture, if the texture coordinates fall outside 0.0 and 1.0.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Original Author:    Mark J. Kilgard
    ///     </para>
    ///     <para>
    ///         C# Implementation:  Randy Ridge
    ///         http://www.randyridge.com/Tao/Default.aspx
    ///     </para>
    /// </remarks>
    #endregion Class Documentation
    public sealed class CheckerOld {
        // --- Fields ---
        #region Private Constants
        private const int CHECKIMAGEWIDTH = 64;
        private const int CHECKIMAGEHEIGHT = 64;
        #endregion Private Constants

        #region Private Fields
        private static byte[ , , ] checkImage = new byte[CHECKIMAGEHEIGHT, CHECKIMAGEWIDTH, 3];
        #endregion Private Fields

        // --- Entry Point ---
        #region Main(string[] args)
        public static void Main(string[] args) {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_RGB | Glut.GLUT_DEPTH);
            Glut.glutCreateWindow("CheckerOld");
            Init();
            Glut.glutDisplayFunc(new Glut.DisplayCallback(Display));
            Glut.glutKeyboardFunc(new Glut.KeyboardCallback(Keyboard));
            Glut.glutReshapeFunc(new Glut.ReshapeCallback(Reshape));
            Glut.glutMainLoop();
        }
		#endregion Main(string[] args)

        // --- Application Methods ---
        #region Init()
        private static void Init() {
            Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glDepthFunc(Gl.GL_LESS);

            MakeCheckImage();
            Gl.glPixelStorei(Gl.GL_UNPACK_ALIGNMENT, 1);
            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, 3, CHECKIMAGEWIDTH, CHECKIMAGEHEIGHT, 0, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, checkImage);
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_CLAMP);
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_CLAMP);
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameterf(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_DECAL);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glShadeModel(Gl.GL_FLAT);
        }
        #endregion Init()

        #region MakeCheckImage()
        private static void MakeCheckImage() {
            int i, j, c;

            for(i = 0; i < CHECKIMAGEHEIGHT; i++) {
                for(j = 0; j < CHECKIMAGEWIDTH; j++) {
                    if(((i & 0x8) == 0) ^ ((j & 0x8) == 0)) {
                        c = 255;
                    }
                    else {
                        c = 0;
                    }
                    checkImage[i, j, 0] = (byte) c;
                    checkImage[i, j, 1] = (byte) c;
                    checkImage[i, j, 2] = (byte) c;
                }
            }
        }
        #endregion MakeCheckImage()

        // --- Callbacks ---
        #region Display()
        private static void Display() {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glBegin(Gl.GL_QUADS);
                Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(-2.0f, -1.0f, 0.0f);
                Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(-2.0f,  1.0f, 0.0f);
                Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f( 0.0f,  1.0f, 0.0f);
                Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f( 0.0f, -1.0f, 0.0f);

                Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3f(1.0f,     -1.0f,  0.0f);
                Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3f(1.0f,      1.0f,  0.0f);
                Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3f(2.41421f,  1.0f, -1.41421f);
                Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3f(2.41421f, -1.0f, -1.41421f);
            Gl.glEnd();
            Glut.glutSwapBuffers();
        }
        #endregion Display()

        #region Keyboard(byte key, int x, int y)
        private static void Keyboard(byte key, int x, int y) {
            switch(key) {
                case (byte) 27:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
        #endregion Keyboard(byte key, int x, int y)

        #region Reshape(int w, int h)
        private static void Reshape(int w, int h) {
            Gl.glViewport(0, 0, w, h);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(60.0, (float) w / (float) h, 1.0, 30.0);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glTranslatef(0.0f, 0.0f, -3.6f);
        }
        #endregion Reshape(int w, int h)
    }
}
